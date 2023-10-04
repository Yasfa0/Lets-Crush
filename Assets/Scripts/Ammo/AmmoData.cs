using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoData
{
    public int ammoID;
    public string ammoName;
    public int ammoCount;
    public bool isInfinite;

    public void CreateFromScriptable(AmmoScriptable script)
    {
        ammoID = script.ammoID;
        ammoName = script.ammoName;
        ammoCount = 0;
        isInfinite = script.isInfinite;
    }
}
