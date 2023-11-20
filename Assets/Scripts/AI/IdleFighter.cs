using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleFighter : State
{
    public IdleFighter(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.IdleFighter;
    }

    public override void Enter()
    {
        base.Enter();
        anim.SetInteger("animState", 0);

    }

    public override void Update()
    {

        Debug.Log(npc.gameObject.name + " STATE is Idle Fighter");
        //If opponent is within vision range and not hidden, chase
        //If opponent is within scan range, shoot.
        if (CanSeeOpponent() && !GetOppHideStats())
        {
            nextState = new Pursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
        else if (CanScanOpponent())
        {
            nextState = new FighterShoot(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }else{
            //nextState = new MoveToNeutral(npc, opposingTeam, anim, agent);
            nextState = new MoveToBenteng(npc,opposingTeam,anim,agent);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class MoveToBenteng : State
{
    GameObject targetBenteng;

    public MoveToBenteng(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.MoveToBenteng;
        agent.speed = 3;
        agent.isStopped = false;

        if (npc.gameObject.tag == "Enemy")
        {
            targetBenteng = MapManager.Instance.GetBentengPlayer();
        }
        else
        {
            targetBenteng = MapManager.Instance.GetBentengEnemy();
        }

    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        //npc.transform.rotation = Quaternion.Euler(0, 180, 0);

        anim.SetInteger("animState", 1);
        agent.SetDestination(targetBenteng.transform.position);
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log(npc.gameObject.name + " STATE is Move To Benteng");
        //Debug.Log("Move To Benteng" + targetBenteng.transform.position);
        agent.SetDestination(targetBenteng.transform.position);

        //Kalau ada di radar vision, dan nggak hidden, kejar
        //Kalau ada di radar scan, kejar
        if (CanSeeOpponent() && !GetOppHideStats())
        {
            nextState = new Pursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
        else if (CanScanOpponent())
        {
            nextState = new Pursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }

        if (agent.hasPath)
        {
            //Debug.Log("Has Path");
            if (agent.remainingDistance < scanDist)
            {
                //Debug.Log("Swap Shoot");
                nextState = new FighterShoot(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}


/*public class MoveToNeutral : State
{
    public MoveToNeutral(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.MoveToNeutral;
        agent.speed = 3;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        //npc.transform.rotation = Quaternion.Euler(0, 180, 0);
        List<Transform> neutralPoints = MapManager.Instance.GetNeutralPoints();
        agent.SetDestination(neutralPoints[Random.Range(0,neutralPoints.Count)].position);
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("Move To Benteng" + targetBenteng.transform.position);
        Debug.Log("Move to Neutral");
        //if (WithinCustomDistance(20))
        //{
          //  nextState = new Pursue(npc, opposingTeam, anim, agent);
           // stage = EVENT.EXIT;
        //}

        //Kalau ada di radar vision, dan nggak hidden, kejar
        //Kalau ada di radar scan, kejar
        if (CanSeeOpponent() && !GetOppHideStats())
        {
            nextState = new Pursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
        else if (CanScanOpponent())
        {
            nextState = new Pursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
        

        if (agent.hasPath)
        {
            //Debug.Log("Has Path");
            if (agent.remainingDistance <= 0.5)
            {
                //Debug.Log("Swap Shoot");
                //nextState = new WanderNeutral(npc, player, anim, agent);
                nextState = new IdleFighter(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}*/

public class Pursue : State
{
    public Pursue(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.Pursue;
        agent.speed = 3;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isRunning");
        base.Enter();

        anim.SetInteger("animState", 1);
    }

    public override void Update()
    {
        Debug.Log(npc.gameObject.name + " STATE is Pursue");
        if (oppTarget != null)
        {
            agent.SetDestination(oppTarget.position);
            Debug.Log("Pursue");
        }

        if (agent.hasPath)
        {
            if (CanScanOpponent())
            {
                nextState = new FighterShoot(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;

            }
            else if (!CanSeeOpponent()|| GetOppHideStats())
            {
                nextState = new MoveToBenteng(npc, opposingTeam, anim, agent);
                //nextState = new MoveToNeutral(npc, opposingTeam, anim, agent);
                //nextState = new IdleFighter(npc, opposingTeam, anim, agent);
                //nextState = new Patrol(npc, player, anim, agent);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class FighterShoot : State
{
    float lastShot;
    float shootCD = 0.5f;

    public FighterShoot(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.FighterShoot;
    }

    public override void Enter()
    {
        base.Enter();

        anim.SetInteger("animState", 0);
    }

    public override void Update()
    {
        Debug.Log(npc.gameObject.name + " STATE is Fighter Shoot");
        if (oppTarget != null && oppTarget.GetComponent<CharacterBase>().GetCurrentHP() > 0)
        {
            Debug.Log(npc.gameObject.name + " STATE is FS Execute");
            Vector3 direction = oppTarget.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            direction.y = 0;

            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), 60f * Time.deltaTime);
            //Debug.Log("ANGLE " + angle);
            //float angleLeft = Vector3.Angle(npc.transform.forward, player.position - npc.transform.position);
            if (angle <= 15 && Time.time - lastShot > shootCD)
            {
                lastShot = Time.time;
                npc.GetComponent<CharacterBase>().Shoot();
            }

            /*if (!WithinShootDistance())
            {
                nextState = new BackToBase(npc, player, anim, agent, chaseLimit);
                stage = EVENT.EXIT;
            }*/

            if (!CanScanOpponent() || GetOppHideStats())
            {
                //nextState = new Idle(npc, player, anim, agent);
                //nextState = new IdleFighter(npc, player, anim, agent);
                //nextState = new MoveToNeutral(npc, opposingTeam, anim, agent);

                nextState = new MoveToBenteng(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;
            }
        }
        else
        {
            //nextState = new MoveToNeutral(npc, opposingTeam, anim, agent);

            nextState = new MoveToBenteng(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
       
    }

    public override void Exit()
    {
        base.Exit();
    }
}
