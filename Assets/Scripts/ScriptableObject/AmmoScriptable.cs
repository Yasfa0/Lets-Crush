using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "New Ammo", menuName = "Scriptable Object/Scriptable Ammo")]
public class AmmoScriptable : ScriptableObject
{
    public string ammoName;
    public Sprite ammoIcon;
    public GameObject ammoPrefab;
}
