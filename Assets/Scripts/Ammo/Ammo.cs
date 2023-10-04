using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    protected float range;
    [SerializeField] protected bool isFriendly = true;
    [SerializeField] protected float damage = 20f;
    [SerializeField] protected float damageCap = 35f;
    [SerializeField] protected float speed = 10f;
    protected Vector3 bulletDir;
    protected bool shooting = false;
    protected Vector3 spawnPos;

    private void Awake()
    {
        spawnPos = transform.position;    
    }

    public void SetTargetPos(Vector3 bulletDir,float range)
    {
        this.bulletDir = bulletDir;
        this.range = range;
        shooting = true;
    }

    private void Update()
    {
        if (bulletDir != null && shooting)
        {
            transform.Translate(bulletDir * speed * Time.deltaTime);
            if (Vector3.Distance(transform.position,spawnPos) >= (range*2))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy" && isFriendly)
        {
            HitEnemy(collision.gameObject);
            Debug.Log("Hit Enemy");
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            HitObstacle(collision.gameObject);
            Debug.Log("Hit Obstacle");
        }
        else if (collision.gameObject.tag == "Benteng")
        {
            HitBenteng(collision.gameObject);
            Debug.Log("Hit Benteng");
        }
        else if(collision.gameObject.tag == "Player" && !isFriendly || collision.gameObject.tag == "Friendly" && !isFriendly)
        {
            HitPlayer(collision.gameObject);
            Debug.Log("Hit Player");
        }
    }

    public virtual float DamageCalc()
    {
        return Random.Range(damage,damageCap);
    }

    public virtual void HitPlayer(GameObject player)
    {
        player.GetComponent<Player>().TakeDamage(DamageCalc());
        Destroy(gameObject);
    }

    public virtual void HitEnemy(GameObject enemy)
    {
        enemy.GetComponent<CharacterBase>().TakeDamage(DamageCalc());
        Destroy(gameObject);
    }

    public virtual void HitObstacle(GameObject obstacle)
    {
        Destroy(gameObject);
    }

    public virtual void HitBenteng(GameObject benteng)
    {
        benteng.GetComponent<Benteng>().TakeDamage(DamageCalc());
        Destroy(gameObject);
    }

}
