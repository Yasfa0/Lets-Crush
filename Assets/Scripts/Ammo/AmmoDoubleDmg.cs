using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDoubleDmg : Ammo
{
    public override float DamageCalc()
    {
        float baseDamage = Random.Range(damage, damageCap);

        float chanceResult = Random.Range(1, 100);
        if (chanceResult <= 40)
        {
            return baseDamage * 2;
        }
        return baseDamage;

    }

}
