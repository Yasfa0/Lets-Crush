using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOneObjective : ObjectiveManager
{
    [SerializeField] private GameObject bossPrefab;
    private List<PhaseSummon> currentPhaseSummon = new List<PhaseSummon>();
    [SerializeField] private List<PhaseSummon> phaseOneSummon = new List<PhaseSummon>();
    [SerializeField] private List<PhaseSummon> phaseTwoSummon = new List<PhaseSummon>();

    protected float nextSummon;

    private void Awake()
    {
        base.Awake();
        SetupPhaseSummon();
    }

    private void Update()
    {
        for (int i = 0; i < currentPhaseSummon.Count; i++)
        {
            if (Time.time >= currentPhaseSummon[i].nextSummon && currentPhaseSummon[i].maxWave > 0)
            {
                currentPhaseSummon[i].nextSummon = Time.time + currentPhaseSummon[i].cooldown;

                Debug.Log("Summon");
                List<GameObject> spawnPointList = MapManager.Instance.GetEnemyPosts();
                for (int j = 0; j < currentPhaseSummon[i].spawnAmount; j++)
                {
                    int rand = Random.Range(0, spawnPointList.Count);
                    GameObject tempSummon = Instantiate(currentPhaseSummon[i].summonPrefab);
                    tempSummon.transform.position = spawnPointList[rand].transform.position;
                }
                currentPhaseSummon[i].maxWave -= 1;
                MapManager.Instance.FillTeams();

            }

        }

    }

    public void SetupPhaseSummon()
    {
        switch (currentPhase)
        {
            case 1:
                currentPhaseSummon = phaseOneSummon;
                break;
            case 2:
                currentPhaseSummon = phaseTwoSummon;
                break;
            default:
                break;
        }

        foreach (PhaseSummon phaseSummon in currentPhaseSummon)
        {
            phaseSummon.nextSummon = Time.time + phaseSummon.cooldown;
        }
    }

    protected override void PhaseTransition()
    {
        SetupPhaseSummon();
        if(currentPhase == 2)
        {
            Debug.Log("Summon Boss");
            List<GameObject> spawnPointList = MapManager.Instance.GetEnemyPosts();
            int rand = Random.Range(0, spawnPointList.Count);
            GameObject tempSummon = Instantiate(bossPrefab);
            tempSummon.transform.position = spawnPointList[rand].transform.position;
            MapManager.Instance.FillTeams();
            Camera.main.gameObject.GetComponent<CameraFollowTarget>().FocusCam(tempSummon.transform.position, 5f);
        }
    }

}
