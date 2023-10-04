using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWallAmmo : Ammo
{
    [SerializeField] private int breakLimit = 1;

    public override void HitObstacle(GameObject obstacle)
    {
        Debug.Log("Hit Obstacle");
        if(breakLimit > 0)
        {
            Destroy(obstacle);
            breakLimit--;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void HitPlayer(GameObject player)
    {
        player.GetComponent<CharacterBase>().TakeDamage(DamageCalc());
        //Destroy(gameObject);
    }

    public override void HitEnemy(GameObject enemy)
    {
        enemy.GetComponent<CharacterBase>().TakeDamage(DamageCalc());
        //Destroy(gameObject);
    }
}
