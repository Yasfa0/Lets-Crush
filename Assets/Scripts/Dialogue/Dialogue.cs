using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue 
{
    public string name;
    public Sprite[] charSprite = new Sprite[3];
    public bool[] dimSprite = new bool[3];

    [TextArea(3,15)]
    public string dialogue;
    public Color textColor = Color.white;

}
