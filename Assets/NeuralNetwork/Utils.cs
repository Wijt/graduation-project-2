using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    public static float GiveMutateRate(float chance)
    {
        float number = Random.Range(0.0f, 1.0f);
        if (number <= chance)
        {
            return Random.Range(-Constants.MAX_MUTATION_RATE, Constants.MAX_MUTATION_RATE);
        }
        return 0;
    }

    public static float ReMap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
