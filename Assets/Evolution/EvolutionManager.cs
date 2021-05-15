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

    public int populationSize;

    public UnityEvent ResetFunctions;

    bool shouldReset = false;

    private void Start()
    {
        CreatePopulation(null);
    }

    private void Update()
    {
        if (GetPopulation(true).Count == populationSize && !shouldReset)
        {
            shouldReset = true;
            CreatePopulation(GetFittestObjectBrain());
        }
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
        for (int i = 0; i < populationSize; i++)
        {
            GameObject spanwedObject = Instantiate(smartObject, spawnPoint);
            SmartObject spawnedObjectScript = spanwedObject.GetComponent<SmartObject>();
            
            spawnedObjectScript.brain = brain != null ? brain.Copy() : new NeuralNetwork(networkSize);

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
        
        //Debug.Log("Best: " + population[0].fitness + ", Worst:" + population[population.Count - 1].fitness);
        return population[population.Count - 1].brain;
    }
}
