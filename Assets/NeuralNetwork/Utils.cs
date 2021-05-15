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
            return Random.Range(-1.0f, 1.0f);
        }
        return 0;
    }
}
