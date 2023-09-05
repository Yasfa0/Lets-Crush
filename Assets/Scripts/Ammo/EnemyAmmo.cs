using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAmmo : Ammo
{
    public override void HitPlayer(GameObject player)
    {
        player.GetComponent<CharacterBase>().TakeDamage(DamageCalc());
        Destroy(gameObject);
    }

    public override void HitEnemy(GameObject enemy)
    {

    }
}
