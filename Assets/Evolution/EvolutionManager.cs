using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvolutionManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject smartObject;
    public int[] networkSize;

    [SerializeField]
    public List<SmartObject> population;

    public int totalPopulationSize;
    [Range(0, 100)]
    public int newBornRatio;

    public SmartObject bestObject;

    public UnityEvent ResetFunctions;
    public UnityEvent LeaveBestFunctions;

    bool shouldReset = false;

    public bool debug;

    private void Start()
    {
        CreatePopulation(null);
        ResetFunctions.AddListener(CheckFittestandTempSave);
    }

    private void Update()
    {
        if (GetPopulation(true).Count == totalPopulationSize && !shouldReset)
        {
            shouldReset = true;
            if (debug)
            {
                Debug.Log("Best Fitness: " + GetFittestObject().fitness);
            }
            CreatePopulation(GetFittestObjectBrain());
        }
    }

    public void LeaveBest()
    {
        shouldReset = true;
        SmartObject fittestObject = GetFittestObject();
        foreach (Transform item in spawnPoint)
        {
            if (item.gameObject != fittestObject.gameObject)
            {
                item.gameObject.SetActive(false);
            }
        }
        LeaveBestFunctions.Invoke();
    }

    public List<SmartObject> GetPopulation(bool isDeath)
    {
        return population.FindAll(e => e.isActive != isDeath);
    }

    public void CreatePopulation(NeuralNetwork brain)
    { 
        population.Clear();
        foreach (Transform child in spawnPoint)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < totalPopulationSize; i++)
        {
            GameObject spanwedObject = Instantiate(smartObject, spawnPoint);
            SmartObject spawnedObjectScript = spanwedObject.GetComponent<SmartObject>();

            if (brain == null || i < totalPopulationSize * (newBornRatio * 0.01))
            {
                spawnedObjectScript.brain = new NeuralNetwork(networkSize);
            }
            else
            {
                spawnedObjectScript.brain = brain.Copy();
            }

            spawnedObjectScript.brain.Mutate();
            population.Add(spawnedObjectScript);
        }
        ResetFunctions.Invoke();
        shouldReset = false;
    }

    public NeuralNetwork GetFittestObjectBrain()
    {
        if (population.Count == 0) return null;

        population.Sort((a, b) => a.fitness.CompareTo(b.fitness));
        
        //Debug.Log("Best: " +population[population.Count - 1].fitness  + ", Worst:" +population[0].fitness );
        return population[population.Count - 1].brain;
    }
    public SmartObject GetFittestObject()
    {
        if (population.Count == 0) return null;

        population.Sort((a, b) => a.fitness.CompareTo(b.fitness));

        //Debug.Log("Best: " + population[population.Count - 1].fitness + ", Worst:" + population[0].fitness);
        return population[population.Count - 1];
    }
    
    public void CheckFittestandTempSave()
    {
        SmartObject fittest = GetFittestObject();
        if (bestObject==null)
        {
            newFittest.brain = fittest.brain.Copy();
            newFittest.fitness = fittest.fitness;
            bestObject = newFittest;
            return;
        }

        if (bestObject.fitness < fittest.fitness)
        {
            newFittest.brain = fittest.brain.Copy();
            newFittest.fitness = fittest.fitness;
        }
    }
}
