using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveBox : MonoBehaviour
{
    [SerializeField] private Text objectiveText;
    private Objective objective;
    private string shownString;

    private void Update()
    {
        if (objective != null)
        {
            UpdateText();
        }
    }

    public void SetupObjBox(Objective inObj)
    {
        InsertObjective(inObj);
        UpdateText();
    }

    public void UpdateText()
    {
        string currentCount = "0";
        switch (objective.objectiveType)
        {
            case GoalType.KnockEnemy:
                currentCount = FindObjectOfType<ObjectiveManager>().GetEnemyKnock().ToString();
                break;
            case GoalType.CaptureEnemy:
                currentCount = FindObjectOfType<ObjectiveManager>().GetEnemyCapture().ToString();
                break;
            case GoalType.KnockBoss:
                currentCount = FindObjectOfType<ObjectiveManager>().GetBossKnock().ToString();
                break;
            case GoalType.CaptureBoss:
                currentCount = FindObjectOfType<ObjectiveManager>().GetBossCapture().ToString();
                break;
            case GoalType.DefendTower:
                if (!FindObjectOfType<ObjectiveManager>().GetPlayerTowerDestroyed())
                {
                    currentCount = "1";
                }
                break;
            case GoalType.DestroyTower:
                if (FindObjectOfType<ObjectiveManager>().GetEnemyTowerDestroyed())
                {
                    currentCount = "1";
                }
                break;
            case GoalType.AcquireItem:
                currentCount = FindObjectOfType<ObjectiveManager>().GetItemAcquired().ToString();
                break;
            default:
                break;
        }

        shownString = objective.description + " " + currentCount + "/" + objective.countTarget.ToString();

        objectiveText.text = shownString;
    }

    public void InsertObjective(Objective objective)
    {
        this.objective = objective;
    }
}