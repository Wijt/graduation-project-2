using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perceptron
{
    float[] weights;
    float bias;

    public Perceptron(int inputCount)
    {
        this.weights = new float[inputCount+1];
        this.bias = 1;

        for (int i = 0; i < inputCount + 1; i++)
        {
            this.weights[i] = Random.Range(-1.0f, 1.0f);
        }
    }
    
    public float Fire(float[] inputs)
    {
        float sum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if (i >= inputs.Length)
                sum += bias * weights[i]; // For the bias
            else
                sum += inputs[i] * weights[i];
        }
        return sum >= 0 ? sum : 0;           //ReLu function
    }
    public Perceptron Copy()
    {
        Perceptron copy = new Perceptron(this.weights.Length - 1);
        for (int i = 0; i < copy.weights.Length; i++)
        {
            copy.weights[i] = this.weights[i];
        }
        return copy;
    }

    public void Mutate(float mutateRate)
    {
        for (int i = 0; i < this.weights.Length; i++)
        {
            this.weights[i] += mutateRate;
        }
    }
}
