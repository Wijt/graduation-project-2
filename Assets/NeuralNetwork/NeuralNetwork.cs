using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    int[] sizes;
    Perceptron[][] perceptrons;

    public NeuralNetwork(int[] sizes)
    {
        this.sizes = sizes;
        this.perceptrons = new Perceptron[sizes.Length - 1][];

        for (int i = 1; i < sizes.Length; i++)
        {
            Perceptron[] layer = new Perceptron[sizes[i]];
            for (int j = 0; j < sizes[i]; j++)
            {
                layer[j] = new Perceptron(sizes[i - 1]);
            }
            perceptrons[i - 1] = layer;
        }
    }

    public float[] FeedForward(float[] inputs)
    {
        float[] previousOutputs = inputs;
        for (int i = 0; i < perceptrons.Length; i++)
        {
            float[] layerOutput = new float[perceptrons[i].Length];
            for (int j = 0; j < perceptrons[i].Length; j++)
            {
                float output = perceptrons[i][j].Fire(previousOutputs);
                layerOutput[j] = output;
            }
            previousOutputs = layerOutput;
        }
        return previousOutputs;
    }

    public NeuralNetwork Copy()
    {
        NeuralNetwork copy = new NeuralNetwork(this.sizes);
        Perceptron[][] cPerceptrons = new Perceptron[this.perceptrons.Length][];
        for (int i = 0; i < this.perceptrons.Length; i++)
        {
            Perceptron[] layer = new Perceptron[perceptrons[i].Length];
            for (int j = 0; j < this.perceptrons[i].Length; j++)
            {
                layer[j] = this.perceptrons[i][j].Copy();
            }
            cPerceptrons[i] = layer;
        }
        copy.perceptrons = cPerceptrons;
        return copy;
    }
    public void Mutate()
    {
        for (int i = 0; i < this.perceptrons.Length; i++)
        {
            for (int j = 0; j < this.perceptrons[i].Length; j++)
            {
                this.perceptrons[i][j].Mutate(Utils.GiveMutateRate(Constants.MUTATION_CHANCE));
            }
        }
    }
}