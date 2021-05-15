using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SmartObject : MonoBehaviour
{
    public NeuralNetwork brain;
    public float fitness = 0;

    public bool isActive = true;
}
