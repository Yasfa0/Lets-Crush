using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosiveAmmo : Ammo
{
    [SerializeField] private GameObject corrosionPrefab;

    public override void HitObstacle()
    {
        Debug.Log("Hit Obstacle");
        GameObject corrosionInst = Instantiate(corrosionPrefab,gameObject.transform.position,Quaternion.identity);
        corrosionInst.transform.parent = null;
        corrosionInst.transform.rotation = Quaternion.Euler(0, 0, 0);
        corrosionInst.transform.position = new Vector3(corrosionInst.transform.position.x, 0.1f, corrosionInst.transform.position.z);
        Destroy(gameObject);
    }

    public override void HitPlayer(GameObject player)
    {
        player.GetComponent<CharacterBase>().TakeDamage(DamageCalc());
        player.GetComponent<CharacterBase>().StartCorrosion(3);
        Destroy(gameObject);
    }

    public override void HitEnemy(GameObject enemy)
    {
        enemy.GetComponent<CharacterBase>().TakeDamage(DamageCalc());
        enemy.GetComponent<CharacterBase>().StartCorrosion(3);
        Destroy(gameObject);
    }

}
