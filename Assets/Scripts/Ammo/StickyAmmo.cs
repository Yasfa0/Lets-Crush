using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyAmmo : Ammo
{
    public override void HitEnemy(GameObject enemy)
    {
        enemy.GetComponent<CharacterBase>().TakeDamage(DamageCalc());
        enemy.GetComponent<Enemy>().StickyHit(5f);
        Destroy(gameObject);
    }
}
