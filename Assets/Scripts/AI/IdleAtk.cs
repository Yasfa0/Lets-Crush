using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleAtk : State
{
    public IdleAtk(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.IdleAtk;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        //npc.transform.rotation = Quaternion.Euler(0, 180, 0);
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log("IdleAtk");
        if (Random.Range(0, 100) < 10)
        {
            nextState = new MoveToBenteng(npc, opposingTeam, anim, agent);
            stage = EVENT.EXIT;
        }
    }

    public bool PlayerEnterZone()
    {
        List<GameObject> charaList = MapManager.Instance.GetEnemyBaseChecker().GetDetectedCharaList();
        foreach (GameObject chara in charaList)
        {
            //Debug.Log(chara.name);
            //Debug.Log(player.gameObject.name);
            if (chara == oppTarget.gameObject)
            {
                return true;
            }
        }
        //Debug.Log("Player undetected");
        return false;
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
        agent.speed = 7;
        agent.isStopped = false;
        targetBenteng = MapManager.Instance.GetBentengPlayer();
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        //npc.transform.rotation = Quaternion.Euler(0, 180, 0);
        agent.SetDestination(targetBenteng.transform.position);
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("Move To Benteng" + targetBenteng.transform.position);

        if (agent.hasPath)
        {
            //Debug.Log("Has Path");
            if (agent.remainingDistance < weapRange)
            {
                //Debug.Log("Swap Shoot");
                nextState = new ShootAtk(npc, opposingTeam, anim, agent);
                stage = EVENT.EXIT;
            }
        }
    }

    public bool PlayerEnterZone()
    {
        List<GameObject> charaList = MapManager.Instance.GetEnemyBaseChecker().GetDetectedCharaList();
        foreach (GameObject chara in charaList)
        {
            //Debug.Log(chara.name);
            //Debug.Log(player.gameObject.name);
            if (chara == oppTarget.gameObject)
            {
                return true;
            }
        }
        //Debug.Log("Player undetected");
        return false;
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class ShootAtk : State
{
    float lastShot;
    float shootCD = 0.5f;

    GameObject targetBenteng;

    public ShootAtk(GameObject _npc, List<Transform> _oppTeam, Animator _anim, NavMeshAgent _agent) : base(_npc, _oppTeam, _anim, _agent)
    {
        name = STATE.Shoot;
        agent.speed = 0;
        agent.isStopped = true;
        targetBenteng = MapManager.Instance.GetBentengPlayer();
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        //npc.transform.rotation = Quaternion.Euler(0, 180, 0);
        agent.velocity = Vector3.zero;
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log("Shoot Atk");
        Vector3 direction = targetBenteng.transform.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), 60f * Time.deltaTime);


        //float angleLeft = Vector3.Angle(npc.transform.forward, player.position - npc.transform.position);
        if (angle < 10 && Time.time - lastShot > shootCD)
        {
            lastShot = Time.time;
            npc.GetComponent<EnemyAtk>().Shoot();
        }

        /*if (!WithinShootDistance())
        {
            nextState = new BackToBase(npc, player, anim, agent, chaseLimit);
            stage = EVENT.EXIT;
        }*/

        /*if (!WithinDistance())
        {
            //nextState = new Idle(npc, player, anim, agent);
            nextState = new BackToBase(npc, player, anim, agent, chaseLimit);
            stage = EVENT.EXIT;
        }*/
    }

    public bool PlayerEnterZone()
    {
        List<GameObject> charaList = MapManager.Instance.GetEnemyBaseChecker().GetDetectedCharaList();
        foreach (GameObject chara in charaList)
        {
            //Debug.Log(chara.name);
            //Debug.Log(player.gameObject.name);
            if (chara == oppTarget.gameObject)
            {
                return true;
            }
        }
        //Debug.Log("Player undetected");
        return false;
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}
