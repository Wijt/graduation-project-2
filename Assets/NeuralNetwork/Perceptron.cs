using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Perceptron
{
    public float[] weights;
    float bias;

    System.Func<float, float> activationFunc;

    public Perceptron(int inputCount)
    {
        this.weights = new float[inputCount+1];
        this.bias = 1;

        for (int i = 0; i < inputCount + 1; i++)
        {
            this.weights[i] = Random.Range(-1.0f, 1.0f);
        }

        this.activationFunc = Constants.ReLU;
    }

    public Perceptron(int inputCount, System.Func<float, float> activationFunc)
    {
        this.weights = new float[inputCount + 1];
        this.bias = 1;

        for (int i = 0; i < inputCount + 1; i++)
        {
            this.weights[i] = Random.Range(-1.0f, 1.0f);
        }
        this.activationFunc = activationFunc;
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
        return this.activationFunc(sum);           //ReLu function
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

    public void Train(float[] inputs, float[] outputs)
    {
        float answer = this.Fire(inputs);
        float orj = outputs[0];
        float error = orj - answer;
        for (int i = 0; i < this.weights.Length; i++)
        {
            if (i >= inputs.Length)
                this.weights[i] += error * 1 * Constants.MAX_MUTATION_RATE;
            else
                this.weights[i] += error * inputs[i] * Constants.MAX_MUTATION_RATE;
        }
    }
}
