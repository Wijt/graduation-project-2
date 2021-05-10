using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System.Collections.Generic;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System;
using System.Xml;
using System.IO;

public class Optimizer : MonoBehaviour {

    //set the number of inputs we are going to use in this Neural Net
    protected int NUM_INPUTS = 3;

    //set the number of outputs we plan to use
    protected int NUM_OUTPUTS = 5;

    public int Trials;//number of trials for each generation
    public float TrialDuration;//how long a trial lasts
    public float StoppingFitness;//at what fitness should we max out at
    bool EARunning;//are we training right now?
    string popFileSavePath, champFileSavePath;
    private bool isSaved = false;//saved flag

    Dictionary<IBlackBox, UnitController> ControllerMap = new Dictionary<IBlackBox, UnitController>();
    private DateTime startTime;//start time of training
    private float timeLeft;//amount of time remaining in this training session
    private float accum;//number of frames accumulated
    private int frames;//frame count
    private float updateInterval = 12;//how often we want to update the frame rate

    private uint Generation;
    private double Fitness;

    SimpleExperiment experiment;
    static NeatEvolutionAlgorithm<NeatGenome> _ea;

    public GameObject Unit;//the smart object
    public bool doTrain = true;//toggle the start and stop training buttons
    public bool canThrottleFPS = false;//toggle whether or not the system can adjust the FPS to get more than the fpsMin frames at a lower timescale
    public float fpsMin = 10;//set the number of minimum frames per second allowed before throttling
    public bool showDebugger = false;//toggle debug logger
    public float timeScale;//used for setting or monitoring the current timescale
    public string trainedObjectID = "bob";//name of file to use / create


    // Use this for initialization
     void Start () {
        accum = 0;
        Utility.DebugLog = showDebugger;
        experiment = new SimpleExperiment();
        XmlDocument xmlConfig = new XmlDocument();
        TextAsset textAsset = (TextAsset)Resources.Load(trainedObjectID+"_experiment.config");
        xmlConfig.LoadXml(textAsset.text);
        experiment.SetOptimizer(this);

        experiment.Initialize(trainedObjectID + " Experiment", xmlConfig.DocumentElement, NUM_INPUTS, NUM_OUTPUTS);

        champFileSavePath = Application.persistentDataPath + string.Format("/{0}.champ.xml", trainedObjectID);
        popFileSavePath = Application.persistentDataPath + string.Format("/{0}.pop.xml", trainedObjectID);

        if (Utility.DebugLog)
        {
            Utility.Log(champFileSavePath);
        }
	}

    // Update is called once per frame
    void Update() 
    { 
    
        Utility.DebugLog = showDebugger;

        if (Time.timeScale != timeScale)
        {
            Time.timeScale = timeScale;
        }

        if (canThrottleFPS && EARunning)
        {
            timeLeft -= Time.deltaTime;
            accum += (Time.timeScale / Time.smoothDeltaTime) * 1.75f; //this provides a much more accurate number with the 1.75 multiplier
            ++frames;

            if (timeLeft <= 0.0)
            {
                var fps = accum / frames;
                timeLeft = updateInterval;
                accum = 0.0f;
                frames = 0;
                if (Utility.DebugLog)
                {
                    Utility.Log(champFileSavePath);
                }
                if (fps < fpsMin)//update the time scale to maximize FPS
                {
                    timeScale = timeScale - 1;
                    Time.timeScale = timeScale;
                    if (Utility.DebugLog)
                    {
                        Utility.Log("Lowering time scale to " + Time.timeScale);
                    }
                }
            }
        }

        //make sure we don't save more than once if the frames are still running and our generation has not updated yet
        if(isSaved && ((Generation - 1) % 50 == 0))
        {
            isSaved = false;
        }

        //autosave
        if (EARunning && (Generation % 50 == 0) && !isSaved)
        {
            Save();
            isSaved = true;
        }
    }

    public void StartEA()
    {
        Restart();
        if (Utility.DebugLog)
        {
            Utility.Log("Starting PhotoTaxis experiment");
            // Utility.Log("Loading: " + popFileLoadPath);
        }
        _ea = experiment.CreateEvolutionAlgorithm(popFileSavePath);
        startTime = DateTime.Now;

        _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);
        _ea.PausedEvent += new EventHandler(ea_PauseEvent);

        Generation = _ea.CurrentGeneration;


        var evoSpeed = timeScale;

        //   Time.fixedDeltaTime = 0.045f;
        Time.timeScale = evoSpeed;
        _ea.StartContinue();
        EARunning = true;
    }

    void ea_UpdateEvent(object sender, EventArgs e)
    {
        Restart();
        if (Utility.DebugLog)
        {
            Utility.Log(string.Format("gen={0:N0} bestFitness={1:N6}",
                _ea.CurrentGeneration, _ea.Statistics._maxFitness));
        }

        Fitness = _ea.Statistics._maxFitness;
        Generation = _ea.CurrentGeneration;

        if (Utility.DebugLog)
        {
            //    Utility.Log(string.Format("Moving average: {0}, N: {1}", _ea.Statistics._bestFitnessMA.Mean, _ea.Statistics._bestFitnessMA.Length));
        }


    }

    void ea_PauseEvent(object sender, EventArgs e)
    {
        timeScale = 1;
        if (Utility.DebugLog)
        {
            Utility.Log("Done ea'ing (and neat'ing)");
        }

        Save();

        DateTime endTime = DateTime.Now;
        if (Utility.DebugLog)
        {
            Utility.Log("Total time elapsed: " + (endTime - startTime));
        }

        TextAsset popTxtAsset = (TextAsset)Resources.Load(popFileSavePath);
        string stream = popTxtAsset.text;


        EARunning = false;
    }

    public void StopEA()
    {

        if (_ea != null && _ea.RunState == SharpNeat.Core.RunState.Running)
        {
            _ea.Stop();
        }
    }

    /// <summary>
    /// save changes
    /// </summary>
    private void Save()
    {
        XmlWriterSettings _xwSettings = new XmlWriterSettings();
        _xwSettings.Indent = true;
        // Save genomes to xml file.        
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath);
        if (!dirInf.Exists)
        {
            if (Utility.DebugLog)
            {
                Debug.Log("Creating subdirectory");
            }
            dirInf.Create();
        }
        using (XmlWriter xw = XmlWriter.Create(popFileSavePath, _xwSettings))
        {
            experiment.SavePopulation(xw, _ea.GenomeList);
        }
        // Also save the best genome

        using (XmlWriter xw = XmlWriter.Create(champFileSavePath, _xwSettings))
        {
            experiment.SavePopulation(xw, new NeatGenome[] { _ea.CurrentChampGenome });
        }
    }

    public void Evaluate(IBlackBox box)
    {
        GameObject obj = Instantiate(Unit, Unit.transform.position, Unit.transform.rotation, this.transform) as GameObject;
        UnitController controller = obj.GetComponent<UnitController>();

        ControllerMap.Add(box, controller);

        controller.Activate(box);
    }

    public void StopEvaluation(IBlackBox box)
    {
        UnitController ct = ControllerMap[box];

        Destroy(ct.gameObject);
    }

    public void RunBest()
    {
        timeScale = 1;

        NeatGenome genome = null;


        // Try to load the genome from the XML document.
        //try
        //{
        //    TextAsset popTxtAsset = (TextAsset)Resources.Load(champFileSavePath);
        //    string stream = popTxtAsset.text;

        //    string xrString = stream;
        //    XmlDocument xdoc = new XmlDocument();
        //    xdoc.LoadXml(xrString);

        //    using (XmlReader xr = new XmlNodeReader(xdoc))
        //        genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, (NeatGenomeFactory)experiment.CreateGenomeFactory())[0];


        //}
        try
        {
            using (XmlReader xr = XmlReader.Create(champFileSavePath))
                genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, (NeatGenomeFactory)experiment.CreateGenomeFactory())[0];


        }
        catch (Exception e1)
        {
            Debug.LogError(" Error loading genome from file!\nLoading aborted.\n" + e1.Message + "\nin: " + champFileSavePath);
            return;
        }

        // Get a genome decoder that can convert genomes to phenomes.
        var genomeDecoder = experiment.CreateGenomeDecoder();

        // Decode the genome into a phenome (neural network).
        var phenome = genomeDecoder.Decode(genome);
        Restart();
        GameObject obj = Instantiate(Unit, Unit.transform.position, Unit.transform.rotation, this.transform) as GameObject;
        UnitController controller = obj.GetComponent<UnitController>();

        ControllerMap.Add(phenome, controller);

        controller.Activate(phenome);
    }

    public float GetFitness(IBlackBox box)
    {
        if (ControllerMap.ContainsKey(box))
        {
            return ControllerMap[box].GetFitness();
        }
        return 0;
    }

    void OnGUI()
    {
        if (doTrain)
        {
            if (GUI.Button(new Rect(10, 10, 100, 40), "Start EA"))
            {
                StartEA();
            }
            if (GUI.Button(new Rect(10, 60, 100, 40), "Stop EA"))
            {
                StopEA();
            }
        }

        if (GUI.Button(new Rect(10, 110, 100, 40), "Run best"))
        {
            RunBest();
        }

        GUI.Button(new Rect(10, Screen.height - 70, 125, 60), string.Format("Generation: {0}\nFitness: {1:0.00}", Generation, Fitness));
    }

    public virtual void Restart()
    {
        if (showDebugger)
        {
            Debug.Log("Restarted");
        }
    }
}
