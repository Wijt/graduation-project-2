using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System.Collections.Generic;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System;
using System.Xml;
using System.IO;

public class C_Optimizer : Optimizer
{
    C_Optimizer()
    {
        NUM_INPUTS = 5;
        NUM_OUTPUTS = 1;
    }
}
