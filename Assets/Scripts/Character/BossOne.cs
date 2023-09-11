using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossOne : Enemy
{
    private void Awake()
    {

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //player = GameObject.FindGameObjectWithTag("Player");
        SetupHealthBar();
    }

    private new void Start()
    {
        /*opposingTeam.Add(GameObject.FindGameObjectWithTag("Player"));
        foreach (GameObject oppChar in GameObject.FindGameObjectsWithTag("Friendly"))
        {
            opposingTeam.Add(oppChar);
        }*/

        opposingTeam = MapManager.Instance.GetFriendlyTeam();
        hpBarInstance.SetupAmmoBar(ammoCapacity, regenCD);
        idleState = new IdleFighter(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        currentState = new IdleFighter(gameObject, ConvertToTransform(opposingTeam), anim, agent);

    }

    public override void Knockdown()
    {
        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            //hpBarInstance.DestroyHealthBar();
            FindObjectOfType<ObjectiveManager>().AddBossKnock();
            isKnocked = true;
            currentState = new Knockdown(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        }
    }

}
