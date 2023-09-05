using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : CharacterBase
{
    [SerializeField] protected float regenCD = 2f;
    protected State idleState;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected State currentState;
    //protected GameObject player;
    protected List<GameObject> opposingTeam = new List<GameObject>();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //player = GameObject.FindGameObjectWithTag("Player");
        SetupHealthBar();
    }

    protected void Start()
    {
        /*foreach (GameObject oppChar in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            opposingTeam.Add(oppChar);
        }*/

        opposingTeam = MapManager.Instance.GetEnemyTeam();
        Debug.Log("Opposing " + opposingTeam[0]);
        hpBarInstance.SetupAmmoBar(ammoCapacity, regenCD);
        idleState = new Idle(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        currentState = new Idle(gameObject, ConvertToTransform(opposingTeam), anim, agent);
    }

    protected new void Update()
    {
        base.Update();
        currentState = currentState.Process();
        Debug.Log(gameObject.name + " " + currentState);
        Knockdown();
    }

    public override void Knockdown()
    {
        if (currentHP <= 0)
        {
            //hpBarInstance.DestroyHealthBar();
            isKnocked = true;
            currentState = new Knockdown(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        }
    }

    public void StickyHit(float duration)
    {

        currentState = new Sticky(gameObject, ConvertToTransform(opposingTeam), anim, agent, duration, idleState);
    }

}
