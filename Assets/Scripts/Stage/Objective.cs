using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective 
{
    public string description;
    public GoalType objectiveType;
    public int countTarget;
    public bool isComplete = false;

    public Objective(string objText, GoalType type, int count)
    {
        description = objText;
        objectiveType = type;
        countTarget = count;
    }
}
