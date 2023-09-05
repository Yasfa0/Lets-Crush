using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAtk : Enemy
{
    private void Awake()
    {

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //player = GameObject.FindGameObjectWithTag("Player");
        idleState = new IdleAtk(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        currentState = new IdleAtk(gameObject, ConvertToTransform(opposingTeam), anim, agent);
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
        idleState = new Idle(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        currentState = new Idle(gameObject, ConvertToTransform(opposingTeam), anim, agent);
    }
}
