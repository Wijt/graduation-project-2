using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Optimizer : Optimizer
{
    public F_PipeSpawner pipeSpawner;
    F_Optimizer()
    {
        NUM_INPUTS = 5;
        NUM_OUTPUTS = 1;
    }

    public override void Restart()
    {
        base.Restart();
        pipeSpawner.SpawnMultiplePipe(3);
    }

}
