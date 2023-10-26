using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleBossOne : State
{
    public IdleBossOne(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.IdleFighter;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log("IdleBossOne");

        //Kalau ada di radar vision, dan nggak hidden, kejar
        //Kalau ada di radar scan, kejar
        if (CanSeeOpponent() && !GetOppHideStats())
        {
            nextState = new BossOnePursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
        else if (CanScanOpponent())
        {
            nextState = new BossOnePursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }

        if (Random.Range(0, 100) < 10)
        {
            nextState = new BossOneNeutral(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class BossOneNeutral : State
{
    public BossOneNeutral(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
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
        agent.SetDestination(neutralPoints[Random.Range(0, neutralPoints.Count)].position);
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("Move To Benteng" + targetBenteng.transform.position);
        Debug.Log("Move to Neutral");
        /*if (WithinCustomDistance(20))
        {
            nextState = new Pursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }*/

        //Kalau ada di radar vision, dan nggak hidden, kejar
        //Kalau ada di radar scan, kejar
        if (CanSeeOpponent() && !GetOppHideStats())
        {
            nextState = new BossOnePursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
        else if (CanScanOpponent())
        {
            nextState = new BossOnePursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }


        if (agent.hasPath)
        {
            //Debug.Log("Has Path");
            if (agent.remainingDistance <= 0.5)
            {
                //Debug.Log("Swap Shoot");
                //nextState = new WanderNeutral(npc, player, anim, agent);
                nextState = new IdleBossOne(npc, opposingTeam, anim, agent);
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

public class BossOnePursue : State
{
    public BossOnePursue(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.Pursue;
        agent.speed = 3;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        if (oppTarget != null)
        {
            agent.SetDestination(oppTarget.position + new Vector3(0,0,3));
            Debug.Log("Pursue");
        }

        if (agent.hasPath)
        {
            if (CanScanOpponent())
            {
                nextState = new BossOneShoot(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;

            }
            else if (!CanSeeOpponent() || GetOppHideStats())
            {
                nextState = new BossOneNeutral(npc, opposingTeam, anim, agent);
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

public class BossOneShoot : State
{
    float lastShot;
    float shootCD = 0.5f;

    public BossOneShoot(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.FighterShoot;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (oppTarget != null && oppTarget.GetComponent<CharacterBase>().GetCurrentHP() > 0)
        {
            Vector3 direction = oppTarget.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            direction.y = 0;
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), 60f * Time.deltaTime);


            //float angleLeft = Vector3.Angle(npc.transform.forward, player.position - npc.transform.position);
            if (angle < 10 && Time.time - lastShot > shootCD)
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
                nextState = new BossOneNeutral(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;
            }
        }
        else
        {
            nextState = new BossOneNeutral(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
