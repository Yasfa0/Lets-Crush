using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE { Idle, IdleAtk, IdleFighter, MoveToNeutral, Pursue, MoveBlock, BackToBase, MoveToBenteng, Shoot, 
        FighterShoot,KnockAttack, Knockdown, KnockCapture, Imprisoned, Sticky }
    public enum EVENT { ENTER, UPDATE, EXIT }

    public STATE name;
    protected EVENT stage;
    protected State nextState;

    protected GameObject npc;
    protected List<Transform> opposingTeam = new List<Transform>();
    protected Transform oppTarget;
    protected Animator anim;
    protected NavMeshAgent agent;

    protected float visDist = 8f;

    //It's currently set to 60 degrees 
    //Need confirmation
    protected float visAngle = 30f;

    protected float scanDist = 5;


    protected GameObject ammoPrefab;
    protected Transform bulletSpawner;

    public State(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent)
    {
        npc = _npc;
        //player = _player;
        opposingTeam = _oppTeam;
        anim = _anim;
        agent = _agent;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { oppTarget = CalcOppClosest(); stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }


    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    public STATE GetCurrentSTATE()
    {
        return name;
    }

    //Calculate closest target
    public Transform CalcOppClosest()
    {
        List<Vector3> directions = new List<Vector3>();
        float closestRange = Mathf.Infinity;
        List<Transform> livingOpponent = new List<Transform>();
        int closestIndex = 0;
        List<GameObject> oppLists = new List<GameObject>();
        GameObject oppBenteng;

        MapManager.Instance.FillTeams();
        if(agent.gameObject.tag == "Enemy")
        {
            oppLists = MapManager.Instance.GetFriendlyTeam();
            oppBenteng = MapManager.Instance.GetBentengPlayer();
        }
        else
        {
            oppLists = MapManager.Instance.GetEnemyTeam();
            oppBenteng = MapManager.Instance.GetBentengEnemy();
        }

        //Check if opposing character is alive
        foreach (GameObject opp in oppLists)
        {
            if (opp.GetComponent<CharacterBase>().GetCurrentHP() > 0)
            {
                livingOpponent.Add(opp.transform);
            }
        }

        foreach (Transform opp in livingOpponent)
        {
            directions.Add(opp.position - npc.transform.position);
        }

        directions.Add(oppBenteng.transform.position - npc.transform.position);

        for (int i = 0; i < directions.Count; i++)
        {
            if (directions[i].magnitude < closestRange)
            {
                closestRange = directions[i].magnitude;
                closestIndex = i;
            }
        }

        if (livingOpponent.Count>0)
        {
            livingOpponent.Add(oppBenteng.transform);
            return livingOpponent[closestIndex];
        }
        else
        {
            return null;
        }
    }

    //Radar Vision
    public bool CanSeeOpponent()
    {
        oppTarget = CalcOppClosest();
        if (oppTarget)
        {
            Vector3 direction = oppTarget.position - npc.transform.position;

            float angle = Vector3.Angle(direction, npc.transform.forward);

            if (direction.magnitude < visDist && angle <= visAngle)
            {
                return true;
            }
        }

        return false;
    }

    //Check hide status
    public bool GetOppHideStats()
    {
        oppTarget = CalcOppClosest();
        return oppTarget.GetComponent<CharacterBase>().GetHidden();
    }

    //Radar Scan
    public bool CanScanOpponent()
    {
        oppTarget = CalcOppClosest();
        if (oppTarget)
        {
            Vector3 direction = oppTarget.position - npc.transform.position;

            if (direction.magnitude < scanDist)
            {
                return true;
            }
        }

        return false;
    }

    public bool OpponentEnterZone()
    {
        oppTarget = CalcOppClosest();
        List<GameObject> charaList = new List<GameObject>();

        if (agent.gameObject.tag == "Enemy")
        {
            charaList = MapManager.Instance.GetEnemyBaseChecker().GetDetectedCharaList();
        }
        else
        {
            charaList = MapManager.Instance.GetPlayerBaseChecker().GetDetectedCharaList();
        }

        
        foreach (GameObject chara in charaList)
        {
            //Debug.Log(chara.name);
            //Debug.Log(player.gameObject.name);
            if (oppTarget != null && chara == oppTarget.gameObject)
            {
                Debug.Log("Character enter base Ally");
                return true;
            }
        }
        //Debug.Log("Player undetected");
        Debug.Log("Nothing enter");
        return false;
    }

    /*public bool WithinShootDistance()
    {
        oppTarget = CalcOppClosest();
        Vector3 direction = oppTarget.position - npc.transform.position;

        if (direction.magnitude <= weapRange)
        {
            return true;
        }
        return false;
    }*/

    /*public bool WithinDistance()
    {
        oppTarget = CalcOppClosest();
        Vector3 direction = oppTarget.position - npc.transform.position;
        
        if (direction.magnitude <= visDist)
        {
            return true;
        }
        return false;
    }*/

    /*public bool WithinCustomDistance(float distance)
    {
        oppTarget = CalcOppClosest();
        if(oppTarget != null)
        {
            Vector3 direction = oppTarget.position - npc.transform.position;

            if (direction.magnitude <= distance)
            {
                //Debug.Log("Within Distance");
                return true;
            }
        }
        return false;
    }*/

    /*public bool CanAttackPlayer()
    {
        if(oppTarget != null)
        {
            oppTarget = CalcOppClosest();
            Vector3 direction = oppTarget.position - npc.transform.position;
            if (direction.magnitude < weapRange)
            {
                return true;
            }
        }
        return false;
    }*/
}

public class Idle : State
{
    bool enteredBase = false;
    Transform chaseLimit;

    public Idle(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.Idle;
        if (opposingTeam[0].gameObject.tag == "Enemy")
        {
            chaseLimit = MapManager.Instance.GetFriendlyChaseLimit();
        }
        else
        {
            chaseLimit = MapManager.Instance.GetEnemyChaseLimit();
        }
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        //npc.transform.rotation = Quaternion.Euler(0, 180, 0);
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("Idling");
        if (OpponentEnterZone())
        {
            enteredBase = true;
        }

        if (enteredBase)
        {
            nextState = new MoveBlock(npc, opposingTeam, anim, agent,chaseLimit);
            stage = EVENT.EXIT; 
        }

        /*if (!WithinDistance())
        {
            //nextState = new Idle(npc, player, anim, agent);
            nextState = new BackToBase(npc, player, anim, agent, chaseLimit);
            stage = EVENT.EXIT;
        }*/
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Sticky : State
{
    float stickyDuration;
    float startSticky;
    State recoveryState;

    public Sticky(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent, float duration, State returnState) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.Sticky;
        stickyDuration = duration;
        recoveryState = returnState;
    }

    public override void Enter()
    {
        startSticky = Time.time;
        //anim.SetTrigger("isIdle");
        //npc.transform.rotation = Quaternion.Euler(0, 180, 0);
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log("Sticky");

        if (Time.time - startSticky >= stickyDuration)
        {
            nextState = recoveryState;
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}
public class MoveBlock : State
{
    bool enteredBase = false;
    protected Transform chaseLimit;

    public MoveBlock(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent, Transform limit) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.MoveBlock;
        chaseLimit = limit;
    }

    public override void Enter()
    {
        oppTarget = CalcOppClosest();
        base.Enter();
    }

    public override void Update()
    {
        enteredBase = OpponentEnterZone();
        oppTarget = CalcOppClosest();
        Vector3 target = new Vector3(oppTarget.position.x,oppTarget.position.y,Mathf.Clamp(oppTarget.position.z, oppTarget.position.z, chaseLimit.position.z));

        agent.SetDestination(target);

        //if (WithinShootDistance())
        if(CanScanOpponent())
        {
            Debug.Log("IF KNOCK");
            nextState = new KnockAttack(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }

        if (agent.hasPath)
        {
            if (!CanScanOpponent() && !enteredBase)
            {
                //nextState = new Idle(npc, player, anim, agent);
                nextState = new BackToBase(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}

public class BackToBase : State
{
    public BackToBase(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.BackToBase;
    }

    public override void Enter()
    {
        List<LaneSpawn> spawnPosts = MapManager.Instance.GetEnemyPosts();
        agent.SetDestination(spawnPosts[Random.Range(0, spawnPosts.Count)].spawnTrans.position);
        base.Enter();
    }

    public override void Update()
    {
       
        if (agent.hasPath)
        {
            Debug.Log("Back");
            if (agent.remainingDistance < 1)
            {
                nextState = new Idle(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;
            }
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }

}
public class KnockAttack : State
{
    float lastShot;
    float shootCD = 0.5f;
    Transform chaseLimit;


    public KnockAttack(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.KnockAttack;
        if (opposingTeam[0].gameObject.tag == "Enemy")
        {
            chaseLimit = MapManager.Instance.GetFriendlyChaseLimit();
        }
        else
        {
            chaseLimit = MapManager.Instance.GetEnemyChaseLimit();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = oppTarget.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), 60f * Time.deltaTime);


        //float angleLeft = Vector3.Angle(npc.transform.forward, player.position - npc.transform.position);
        //if (angle < 10 && Time.time - lastShot > shootCD)
        if(CanScanOpponent() && Time.time - lastShot > shootCD)
        {
            lastShot = Time.time;
            Vector3 pushDir = npc.transform.forward * 1000;
            npc.GetComponent<CharacterBase>().Push(oppTarget.gameObject);
            Debug.Log("KNOCK PUSH");
        }

        if (!CanScanOpponent())
        {
            nextState = new MoveBlock(npc,opposingTeam,anim,agent,chaseLimit);
            stage = EVENT.EXIT;
        }

        /*if (!WithinShootDistance())
        {
            nextState = new BackToBase(npc, player, anim, agent, chaseLimit);
            stage = EVENT.EXIT;
        }*/

        /*if (!WithinDistance())
        {
            //nextState = new Idle(npc, player, anim, agent);
            nextState = new BackToBase(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }*/

    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class KnockCapture : State
{
    Transform followTarget;

    public KnockCapture(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent, Transform transTarget) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.KnockCapture;
        followTarget = transTarget;
        agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        //npc.transform.rotation = Quaternion.Euler(0, 180, 0);
        agent.SetDestination(followTarget.position);
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(followTarget.position);
        Debug.Log("Follow Pos: " + followTarget.position);
        Debug.Log("Knock Capture");
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Knockdown : State
{

    public Knockdown(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.Knockdown;
        agent.speed = 0;
        agent.isStopped = true;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        //npc.transform.rotation = Quaternion.Euler(0, 180, 0);
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log("Knockdown");
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Imprisoned : State
{

    public Imprisoned(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.Imprisoned;
        agent.speed = 0;
        agent.isStopped = true;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        //npc.transform.rotation = Quaternion.Euler(0, 180, 0);
        base.Enter();
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}