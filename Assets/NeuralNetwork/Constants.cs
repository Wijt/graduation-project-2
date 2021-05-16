using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static float MUTATION_CHANCE = 0.3f;
    public static float MAX_MUTATION_RATE = 0.1f;
    public static float ReLU(float x)
    {
        return x >= 0 ? x : 0;
    }

    public static float Sign(float x)
    {
        return x >= 0 ? 1 : -1;
    }
}