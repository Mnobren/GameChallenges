using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsBehaviour : WindowBehaviour
{
    public override void Close()
    {
        GameBehaviour.instance.UpdateSettings();
        base.Close();
    }
}
