using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : CharacterBase
{
    [SerializeField] protected float regenCD = 2f;
    protected State idleState;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected State currentState;
    //protected GameObject player;
    protected List<GameObject> opposingTeam = new List<GameObject>();
    protected bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //player = GameObject.FindGameObjectWithTag("Player");
        SetupHealthBar();
    }

    protected void Start()
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

    protected new void Update()
    {
        base.Update();
        currentState = currentState.Process();
        //Knockdown();
    }

    public override void Knockdown()
    {
        if(currentHP <= 0 && !isDead)
        {
            isDead = true;
            //hpBarInstance.DestroyHealthBar();
            FindObjectOfType<ObjectiveManager>().AddEnemyKnock();
            isKnocked = true;
            currentState = new Knockdown(gameObject,ConvertToTransform(opposingTeam),anim,agent);
        }
    }

    public void StickyHit(float duration)
    {

        currentState = new Sticky(gameObject, ConvertToTransform(opposingTeam), anim, agent, duration, idleState);
    }

    public void Captured(Transform follow)
    {
        isDead = false;
        currentState = new KnockCapture(gameObject, ConvertToTransform(opposingTeam), anim, agent, follow);
    }

    public void Imprisoned()
    {
        FindObjectOfType<ObjectiveManager>().AddEnemyCapture();
        currentState = new Imprisoned(gameObject, ConvertToTransform(opposingTeam), anim, agent);
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public void SetCurrentState(State currentState)
    {
        this.currentState = currentState;
    }
}
