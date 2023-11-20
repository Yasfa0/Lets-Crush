using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAmmo : Ammo
{
    public override void HitPlayer(GameObject player)
    {
        player.GetComponent<CharacterBase>().TakeDamage(DamageCalc());
        Destroy(gameObject);
    }

    public override void HitEnemy(GameObject enemy)
    {

    }
    public override void HitObstacle(GameObject obstacle)
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy" && isFriendly)
        {
            HitEnemy(other.gameObject);
            Debug.Log("Hit Enemy");
        }
        else if (other.gameObject.tag == "Obstacle")
        {
            HitObstacle(other.gameObject);
            Debug.Log("Hit Obstacle");
        }
        else if (other.gameObject.tag == "Benteng")
        {
            HitBenteng(other.gameObject);
            Debug.Log("Hit Benteng");
        }
        else if (other.gameObject.tag == "Player" && !isFriendly || other.gameObject.tag == "Friendly" && !isFriendly)
        {
            HitPlayer(other.gameObject);
            Debug.Log("Hit Player");
        }
    }

}
