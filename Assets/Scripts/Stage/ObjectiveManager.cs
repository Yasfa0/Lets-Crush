using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GoalType {KnockEnemy, CaptureEnemy, KnockBoss, AcquireItem }

public class ObjectiveManager : MonoBehaviour
{
    protected int enemyKnocked = 0;
    protected int enemyCapture = 0;
    protected int bossKnocked = 0;
    protected int itemAcquired = 0;

    protected List<Objective> currentPhaseObjective = new List<Objective>();
    [SerializeField] protected List<Objective> phaseOneObjective = new List<Objective>();
    [SerializeField] protected List<Objective> phaseTwoObjective = new List<Objective>();
    [SerializeField] protected List<Objective> hiddenObjective = new List<Objective>();
    protected int currentPhase = 1;

    [SerializeField] protected List<AmmoScriptable> availableReward = new List<AmmoScriptable>();

    [SerializeField] protected GameObject objectiveBoxPrefab;
    [SerializeField] protected GameObject rewardBoxPrefab;
    [SerializeField] protected TextMeshProUGUI phaseText;
    protected List<GameObject> objectiveBoxList = new List<GameObject>();

    protected bool gamePaused = false;

    protected void Awake()
    {
        SetupCurrentObjective();
    }

    public void TogglePause(bool gamePaused)
    {
        this.gamePaused = gamePaused;
        if (this.gamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SetupCurrentObjective()
    {
        //Clear semua box yang ada
        //Instantiate box sebanyak jumlah objective
        //Isi box dengan objective 

        if(objectiveBoxList.Count > 0)
        {
            foreach (GameObject objBox in objectiveBoxList)
            {
                Destroy(objBox);
            }
            objectiveBoxList.Clear();
        }

        FillCurrentPhaseList();

        phaseText.text = "Phase " + currentPhase;

        for (int i = 0; i < currentPhaseObjective.Count; i++)
        {
            GameObject tempBox = Instantiate(objectiveBoxPrefab, transform);
            tempBox.GetComponent<ObjectiveBox>().SetupObjBox(currentPhaseObjective[i]);
            objectiveBoxList.Add(tempBox);
        }

    }

    public void FillCurrentPhaseList()
    {
        switch (currentPhase)
        {
            case 1:
                currentPhaseObjective = phaseOneObjective;
                break;
            case 2:
                currentPhaseObjective = phaseTwoObjective;
                break;
            default:
                break;
        }
    }

    public void CheckObjective()
    {
        //Ambil objective dari dalem phase list, cek satu satu
        //Kalau current number dan target number setidaknya sama, obj itu complete
        //Kalau semua obj dalam satu phase complete, lanjut ke next phase
        //Kalau nggak ada next phase, stage complete

        for (int i = 0; i < currentPhaseObjective.Count; i++)
        {
            if (!currentPhaseObjective[i].isComplete)
            {
                switch (currentPhaseObjective[i].objectiveType)
                {
                    case GoalType.KnockEnemy:
                        if(enemyKnocked >= currentPhaseObjective[i].countTarget)
                        {
                            currentPhaseObjective[i].isComplete = true;
                        }
                        break;
                    case GoalType.CaptureEnemy:
                        if (enemyCapture >= currentPhaseObjective[i].countTarget)
                        {
                            currentPhaseObjective[i].isComplete = true;
                        }
                        break;
                    case GoalType.KnockBoss:
                        if (bossKnocked >= currentPhaseObjective[i].countTarget)
                        {
                            currentPhaseObjective[i].isComplete = true;
                        }
                        break;
                    case GoalType.AcquireItem:
                        if (itemAcquired >= currentPhaseObjective[i].countTarget)
                        {
                            currentPhaseObjective[i].isComplete = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }


        bool allCompleted = true;
        foreach (Objective obj in currentPhaseObjective)
        {
            if (!obj.isComplete)
            {
                allCompleted = false;
            }
        }

        if (allCompleted)
        {
            currentPhase++;
            if(currentPhase > 2)
            {
                //Level Cleared
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                GameObject reward = Instantiate(rewardBoxPrefab, gameObject.transform.parent);
                reward.GetComponent<RewardBox>().GenerateReward(5,availableReward);
                TogglePause(true);
            }
            else
            {
                SetupCurrentObjective();
                PhaseTransition();
            }
        }
    }

    protected virtual void PhaseTransition()
    {
        Debug.Log("Override ini untuk phase specific transition effect");
    }

    public void AddEnemyKnock()
    {
        enemyKnocked += 1;
        CheckObjective();
    }

    public void AddEnemyCapture()
    {
        enemyCapture += 1;
        CheckObjective();
    }
    public void AddBossKnock()
    {
        bossKnocked += 1;
        CheckObjective();
    }

    public void AddItemAcquired()
    {
        itemAcquired += 1;
        CheckObjective();
    }

    public int GetEnemyKnock()
    {
        return enemyKnocked;
    }

    public int GetEnemyCapture()
    {
        return enemyCapture;
    }

    public int GetBossKnock()
    {
        return bossKnocked;
    }

    public int GetItemAcquired()
    {
        return itemAcquired;
    }
}
