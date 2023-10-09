using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossOne : Enemy
{
    private int shotCounter = 0;
    private int randomShotCounter = 0;
    private bool specialAttackUsed = false;
    [SerializeField] private GameObject specialAttack;
    [SerializeField] private List<GameObject> randomAmmoList = new List<GameObject>();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //player = GameObject.FindGameObjectWithTag("Player");
        SetupHealthBar();
    }

    private new void Start()
    {
        /*opposingTeam.Add(GameObject.FindGameObjectWithTag("Player"));
        foreach (GameObject oppChar in GameObject.FindGameObjectsWithTag("Friendly"))
        {
            opposingTeam.Add(oppChar);
        }*/

        opposingTeam = MapManager.Instance.GetFriendlyTeam();
        hpBarInstance.SetupAmmoBar(ammoCapacity, regenCD);
        idleState = new IdleFighter(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        currentState = new IdleFighter(gameObject, ConvertToTransform(opposingTeam), anim, agent);

    }

    public override IEnumerator RoFShot()
    {
        hpBarInstance.ReduceAmmo();
        for (int i = 0; i < rateOfFire; i++)
        {
            GameObject chosenBullet = ammoPrefab;

            if (shotCounter <=3)
            {
                if(shotCounter <= 2)
                {
                    shotCounter++;
                }
                if (shotCounter >= 3 && randomShotCounter <= 2)
                {
                    chosenBullet = randomAmmoList[Random.Range(0, randomAmmoList.Count)];
                    randomShotCounter++;
                }
                else
                {
                    randomShotCounter = 0;
                    shotCounter = 0;

                    if (!specialAttackUsed)
                    {
                        specialAttackUsed = true;
                        chosenBullet = specialAttack;
                    }
                }
            }



            Vector3 bulletDir = transform.forward.normalized * 2;
            lastShot = Time.time;
            GameObject bulletInstance = Instantiate(chosenBullet, bulletSpawner.position, Quaternion.identity);
            bulletInstance.transform.parent = null;
            AudioManagerY.Instance.PlayAudio(audioShoot, 1);
            //bulletInstance.transform.Translate(bulletTarget * 100 * Time.deltaTime);
            bulletInstance.GetComponent<Ammo>().SetTargetPos(bulletDir, range);
            yield return new WaitForSeconds(rofDelay);
        }
    }


    public override void Knockdown()
    {
        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            //hpBarInstance.DestroyHealthBar();
            FindObjectOfType<ObjectiveManager>().AddBossKnock();
            isKnocked = true;
            currentState = new Knockdown(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        }
    }

    public override void Imprisoned()
    {
        FindObjectOfType<ObjectiveManager>().AddBossCapture();
        currentState = new Imprisoned(gameObject, ConvertToTransform(opposingTeam), anim, agent);
    }

}
