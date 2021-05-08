using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrameRate : MonoBehaviour{

    void Awake()
    {
        Application.targetFrameRate = 300;
        QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update () {
        Application.targetFrameRate = 300;
        QualitySettings.vSyncCount = 0;
	}
}
