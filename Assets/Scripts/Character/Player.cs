using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Player : CharacterBase
{
    [SerializeField] private GameObject losePopUp;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private AudioClip audioKlik;
    [SerializeField] private AudioClip loseAudio;
    List<SummonData> summonableAllies = new List<SummonData>();
    List<bool> alreadyUsed = new List<bool>();
    List<float> cooldownList = new List<float>();
    float cdDuration = 20.5f;
    [SerializeField] private List<SummonData> dummySummons = new List<SummonData>();

    private void Awake()
    {
        if(GameData.GetSummonDatas().Count > 0)
        {
            summonableAllies = GameData.GetSummonDatas();
        }
        else
        {
            summonableAllies = dummySummons;
        }

        foreach (SummonData ally in summonableAllies)
        {
            alreadyUsed.Add(false);
            cooldownList.Add(cdDuration);
        }

        for (int i = 0; i < summonableAllies.Count; i++)
        {
            FindObjectOfType<WeaponBar>().SetSlotImg(i, summonableAllies[i].icon);
        }

        SetupHealthBar();
    }

    private new void Update()
    {
        base.Update();
        Knockdown();
        SummonAlly();
        CountdownTimer();
    }

    protected void CountdownTimer()
    {
        for (int i = 0; i < cooldownList.Count; i++)
        {
            if (cooldownList[i] > 0 && alreadyUsed[i])
            {
                cooldownList[i] -= Time.deltaTime;
                FindObjectOfType<WeaponBar>().SetCDText(i, Mathf.FloorToInt(cooldownList[i]%60).ToString());

            }else if (cooldownList[i] <= 0 && alreadyUsed[i])
            {
                alreadyUsed[i] = false;
                FindObjectOfType<WeaponBar>().SetCDText(i, "");
                FindObjectOfType<WeaponBar>().SetSlotImg(i, summonableAllies[i].icon);
            }
        }
    }

    public void SummonAlly()
    {
        int summonIndex = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            summonIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            summonIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            summonIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            summonIndex = 3;
        }

        if(summonIndex >= 0 && summonIndex < alreadyUsed.Count)
        {
            if (!alreadyUsed[summonIndex])
            {
                AudioManagerY.Instance.PlayAudio(audioKlik, 1);
                int rand = Random.Range(0, MapManager.Instance.GetPlayerPosts().Count);

                //Set position
                LaneSpawn selectedLane = MapManager.Instance.GetPlayerPosts()[rand];
                GameObject sumAlly = Instantiate(summonableAllies[summonIndex].allyPrefab, selectedLane.spawnTrans.position,Quaternion.identity, null);
                sumAlly.GetComponent<Ally>().SetSummonIndex(summonIndex);
                cooldownList[summonIndex] = cdDuration;


                //Assign Lane
                int laneMask = sumAlly.GetComponent<NavMeshAgent>().areaMask;
                laneMask = 1 << NavMesh.GetAreaFromName(selectedLane.areaName);
                laneMask += 1 << NavMesh.GetAreaFromName("Walkable");

                sumAlly.GetComponent<NavMeshAgent>().areaMask = laneMask;

                sumAlly.transform.parent = null;
                alreadyUsed[summonIndex] = true;
                FindObjectOfType<WeaponBar>().EraseSlotImg(summonIndex);
                summonIndex = -1;
            }
        }

    }

    public override void Knockdown()
    {
        if(currentHP <= 0)
        {
            GameObject lose = Instantiate(losePopUp, mainUI.transform);
            AudioManagerY.Instance.StopAudioChannel(0);
            AudioManagerY.Instance.PlayNewAudio(loseAudio, 0, true);
            FindObjectOfType<ObjectiveManager>().TogglePause(true);
            gameObject.SetActive(false);
            //Destroy(gameObject);
            ///SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }
}
