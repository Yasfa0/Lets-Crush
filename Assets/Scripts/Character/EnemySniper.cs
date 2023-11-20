using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySniper : Enemy
{
    [SerializeField] private float visDist = 20;
    [SerializeField] private float scanDist = 20;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetupHealthBar();
    }

    private new void Start()
    {
        opposingTeam = MapManager.Instance.GetFriendlyTeam();
        hpBarInstance.SetupAmmoBar(ammoCapacity, regenCD);
        idleState = new IdleSniper(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        currentState = new IdleSniper(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        idleState.SetScanDist(scanDist);
        idleState.SetVisDist(visDist);
    }

}