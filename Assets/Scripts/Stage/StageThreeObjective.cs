using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StageThreeObjective : ObjectiveManager
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private LaneSpawn bossLane;
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
                List<LaneSpawn> spawnPointList = MapManager.Instance.GetEnemyPosts();
                for (int j = 0; j < currentPhaseSummon[i].spawnAmount; j++)
                {
                    int rand = Random.Range(0, spawnPointList.Count);
                    LaneSpawn selectedSpawn = spawnPointList[rand];

                    GameObject tempSummon = Instantiate(currentPhaseSummon[i].summonPrefab, selectedSpawn.spawnTrans.position, Quaternion.identity, null);

                    //Set Position
                    //tempSummon.transform.position = selectedSpawn.spawnTrans.position;
                    //tempSummon.transform.position = new Vector3(tempSummon.transform.position.x, 0.85f, tempSummon.transform.position.z);

                    //Set Lane
                    int laneMask = tempSummon.GetComponent<NavMeshAgent>().areaMask;
                    //laneMask += 1 << NavMesh.GetAreaFromName("Everything");
                    //laneMask = 1;
                    laneMask = 1 << NavMesh.GetAreaFromName(selectedSpawn.areaName);
                    laneMask += 1 << NavMesh.GetAreaFromName("Walkable");
                    tempSummon.GetComponent<NavMeshAgent>().areaMask = laneMask;

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
        if (currentPhase == 2)
        {
            Debug.Log("Summon Boss");

            GameObject tempSummon = Instantiate(bossPrefab);

            tempSummon.transform.position = bossLane.spawnTrans.position;

            int laneMask = tempSummon.GetComponent<NavMeshAgent>().areaMask;
            laneMask = 1 << NavMesh.GetAreaFromName(bossLane.areaName);
            laneMask += 1 << NavMesh.GetAreaFromName("Walkable");

            tempSummon.GetComponent<NavMeshAgent>().areaMask = laneMask;

            MapManager.Instance.FillTeams();
            Camera.main.gameObject.GetComponent<CameraFollowTarget>().FocusCam(tempSummon.transform.position, 5f);
        }
    }
}
