using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhaseSummon
{
    public GameObject summonPrefab;
    public int spawnAmount;
    public float cooldown;
    public int maxWave;
    public float nextSummon;
}
