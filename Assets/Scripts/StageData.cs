using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public string sceneName;

    [TextArea(3,15)]
    public string stageDesc;

    [TextArea(3, 15)]
    public string bossInfo;

    public Sprite stagePreview;

    public bool canBePlayed = false;
}