using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleSniper : State
{
    public IdleSniper(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.IdleSniper;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log("Idle Sniper");

        //If opponent is within vision range and not hidden, chase
        //If opponent is within scan range, shoot.
        /*if (CanSeeOpponent() && !GetOppHideStats())
        {
            nextState = new Pursue(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
        else if (CanScanOpponent())
        {
            nextState = new FighterShoot(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
        else
        {
            //nextState = new MoveToNeutral(npc, opposingTeam, anim, agent);
            nextState = new ClimbClosest(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }*/

        nextState = new ClimbClosest(npc, opposingTeam, anim, agent);
        stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }

}

public class ClimbClosest : State
{
    public ClimbClosest(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.ClimbClosest;
    }

    public override void Enter()
    {
        base.Enter();

        agent.SetDestination(FindClosestWall().transform.position);
    }

    public GameObject FindClosestWall()
    {
        float closestDist = Mathf.Infinity;
        List<GameObject> wallA = new List<GameObject>();
        GameObject currentWall = null;

        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            wallA.Add(wall);
        }

        for (int i = 0; i < wallA.Count; i++)
        {
            Vector3 curDir = wallA[i].gameObject.transform.position - npc.gameObject.transform.position;
            if (curDir.magnitude <= closestDist)
            {
                closestDist = curDir.magnitude;
                currentWall = wallA[i];
            }
        }

        //Debug.Log("CLOSEST WALL: " + currentWall.name);
        return currentWall;
    }

    public override void Update()
    {
        Debug.Log("Sniper To CWall");

        //if (agent.hasPath)
        //{
            if (agent.remainingDistance <= 1)
            {
                //Debug.Log("Swap Shoot");
                nextState = new SniperPatrol(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;
            }
        //}
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }

}

public class SniperPatrol : State
{
    public SniperPatrol(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.SniperPatrol;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log("Sniper Patrol");
        Debug.Log("Can Scan Sniper: " + CanScanOpponent());
        if (CanScanOpponent())
        {
            nextState = new SniperShoot(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }

}


public class SniperShoot : State
{
    float lastShot;
    float shootCD = 0.5f;

    public SniperShoot(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.FighterShoot;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log(npc.gameObject.name + " STATE is Sniper Shoot");
        if (oppTarget != null && oppTarget.GetComponent<CharacterBase>().GetCurrentHP() > 0)
        {
            Debug.Log(npc.gameObject.name + " STATE is SP Execute");
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

                nextState = new SniperPatrol(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;
            }
        }
        else
        {
            //nextState = new MoveToNeutral(npc, opposingTeam, anim, agent);

            nextState = new SniperPatrol(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
