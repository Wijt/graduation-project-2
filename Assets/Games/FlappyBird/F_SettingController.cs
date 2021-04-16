using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_SettingController : MonoBehaviour
{
    public F_GameSettingsController setSetting;
    public static F_GameSettingsController setting;

    private void Awake()
    {
        setting = setSetting;
    }
}

