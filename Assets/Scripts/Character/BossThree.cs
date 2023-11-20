using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossThree : Enemy
{
    //Multi Ammo
    //35s Ability CD

    //Heavy Mode
    //Masuk ketika HP 30%
    //Durasi 10 detik
    //2x damage to ally
    //Immunity (1 Hit)
    //Health Recovery up to 50%

    [SerializeField] private List<GameObject> ammoList = new List<GameObject>();

    //Heavy Mode
    bool isHeavy = false;
    float heavyHealth;
    float lastHeavyEnter;
    bool heavyImmune = false;
    float lastRegen;
    float regenTick = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //player = GameObject.FindGameObjectWithTag("Player");
        SetupHealthBar();
        heavyHealth = (30f / 100f) * maxHP;
        Debug.Log("Max Health: " + maxHP);
        Debug.Log("Heavy Health: " + heavyHealth);
    }

    private new void Start()
    {
        opposingTeam = MapManager.Instance.GetFriendlyTeam();
        hpBarInstance.SetupAmmoBar(ammoCapacity, regenCD);
        idleState = new IdleFighter(gameObject, ConvertToTransform(opposingTeam), anim, agent);
        currentState = new IdleFighter(gameObject, ConvertToTransform(opposingTeam), anim, agent);
    }

    public void Update()
    {
        base.Update();
        if (isHeavy)
        {
            if (currentHP <=  ((50f / 100f) * maxHP) &&   Time.time - lastRegen >= regenTick)
            {
                HealDamage(20);
                Debug.Log("Regen Heal");
                lastRegen = Time.time;
            }
        }
        else if (isHeavy && Time.time - lastHeavyEnter >= 10f)
        {
            isHeavy = false;
            heavyImmune = false;
            Debug.Log("Boss exiting Heavy Mode");
        }
    }

    public override IEnumerator RoFShot()
    {
        hpBarInstance.ReduceAmmo();
        for (int i = 0; i < rateOfFire; i++)
        {
            Vector3 bulletDir = transform.forward.normalized * 2;
            lastShot = Time.time;
            GameObject bulletInstance = Instantiate(ammoList[Random.RandomRange(0,ammoList.Count)], bulletSpawner.position, Quaternion.identity);
            bulletInstance.transform.parent = null;
            AudioManagerY.Instance.PlayAudio(audioShoot, 1);

            Ammo curAmmo = bulletInstance.GetComponent<Ammo>();

            if (isHeavy)
            {
                float capRange = curAmmo.GetDamageCap() - curAmmo.GetDamage();
                curAmmo.SetDamage(curAmmo.GetDamage() * 2);
                curAmmo.SetDamageCap((curAmmo.GetDamage() * 2) + capRange);
            }
            //bulletInstance.transform.Translate(bulletTarget * 100 * Time.deltaTime);
            curAmmo.SetTargetPos(bulletDir, range);
            yield return new WaitForSeconds(rofDelay);
        }
    }

    public override void TakeDamage(float dmgTaken)
    {
        if(!heavyImmune)
        {
            base.TakeDamage(dmgTaken);
        }
        else if(heavyImmune)
        {
            heavyImmune = false;
        }

        if (currentHP <= heavyHealth)
        {
            isHeavy = true;
            heavyImmune = true;
            lastHeavyEnter = Time.time;
            Debug.Log("Boss entering Heavy Mode");
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
