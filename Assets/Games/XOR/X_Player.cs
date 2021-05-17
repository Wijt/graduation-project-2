using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X_Player : SmartObject
{
    public int maxThinkCount = 100;

    int[,] truthTable;

    public int rowID;

    public float output;
    public bool calculate = false;

    // Start is called before the first frame update
    void Start()
    {
        truthTable = new int[,]
        {
            {0,0,0},
            {0,1,1},
            {1,0,1},
            {1,1,0}
        };
        isActive = true;
        fitness = 0;
        for (int i = 0; i < maxThinkCount; i++)
        {
            int truthRowID = Random.Range(0, truthTable.GetLength(0));
            float[] inputs = new float[2];
            inputs[0] = truthTable[truthRowID, 0];
            inputs[1] = truthTable[truthRowID, 1];
            float[] outputs = brain.FeedForward(inputs);
            float diffToTruth = Mathf.Abs(outputs[0] - truthTable[truthRowID, 2]);
            if (diffToTruth < 0.50f)
            {
                fitness += 100;
            }
            else
            {
                fitness -= 100;
            }
            //break;
        }
        isActive = false;
    }
    private void Update()
    {
        if (calculate)
        {
            calculate = false;

            output = brain.FeedForward(new float[] { truthTable[rowID, 0], truthTable[rowID, 1] })[0];
            
            float diffToTruth = Mathf.Abs(output - truthTable[rowID, 2]);
            
            if (diffToTruth < 0.50f)
                Debug.Log("TRUE: " + output);
            else
                Debug.Log("FALSE: " + output);
        }
    }

    
}
