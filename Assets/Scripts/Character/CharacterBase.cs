using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] protected AudioClip audioShoot;
    [SerializeField] protected float maxHP = 1000f;
    [SerializeField] protected GameObject hpBarPrefab;
    [SerializeField] protected Vector3 hpbarOffset = new Vector3(0, 4f, 3.5f);
    [SerializeField] protected Transform bulletSpawner;
    [SerializeField] protected float range = 4f;
    [SerializeField] protected int ammoCapacity = 2;
    [SerializeField] protected int rateOfFire = 2;
    [SerializeField] protected GameObject ammoPrefab;
    protected float lastShot;
    protected float rofDelay = 0.3f;
    protected FloatingHealthBar hpBarInstance;
    protected float currentHP;
    protected bool isKnocked = false;

    protected float corrosionDuration = 0f;
    protected bool isCorroded = false;
    protected float nextCorrosion;

    protected Vector3 pushDirection = new Vector3();
    protected bool isPushed = false;

    protected bool hidden;

    public void SetHidden(bool hidden)
    {
        this.hidden = hidden;
    }

    public bool GetHidden()
    {
        return hidden;
    }

    protected void Update()
    {
        PushProcess();
        CorrosionProcess();
    }

    protected void PushProcess()
    {
        if (isPushed)
        {
            transform.Translate(pushDirection * 10 * Time.deltaTime);
        }
    }

    protected void CorrosionProcess()
    {
        if (isCorroded)
        {
            //Debug.Log("Corroded");
            if (Time.time >= nextCorrosion)
            {
                nextCorrosion = Time.time + 1f;
                corrosionDuration -= 1f;

                TakeDamage(15f);
                Debug.Log("Corrosion Damage");

                if (corrosionDuration <= 0)
                {
                    isCorroded = false;
                }
            }
        }
    }

    public List<Transform> ConvertToTransform(List<GameObject> team)
    {
        List<Transform> transTeam = new List<Transform>();

        foreach (GameObject member in team)
        {
            transTeam.Add(member.transform);
        }

        return transTeam;
    }

    public void StartCorrosion(float duration)
    {
        //Check if already corroded
        if (!isCorroded)
        {
            nextCorrosion = Time.time + 1f;
        }

        isCorroded = true;
        corrosionDuration += duration; 
    }

    public virtual void SetupHealthBar()
    {
        currentHP = maxHP;
        hpBarInstance = Instantiate(hpBarPrefab, transform.position + hpbarOffset, Quaternion.Euler(hpBarPrefab.GetComponent<RectTransform>().localRotation.eulerAngles.x, 0, 0), null).GetComponent<FloatingHealthBar>();
        bool isFriendly = true;
        if (gameObject.tag == "Enemy")
        {
            isFriendly = false;
        }
        hpBarInstance.SetupHealthBar(gameObject, maxHP, currentHP,isFriendly);
    }

    public void Push(GameObject target)
    {
        Vector3 dir = -transform.forward.normalized;
        target.GetComponent<CharacterBase>().StartCoroutine(target.GetComponent<CharacterBase>().ExecutePush(dir));
    }

    public IEnumerator ExecutePush(Vector3 dir)
    {
        if (!isPushed)
        {
            pushDirection = dir;
            isPushed = true;
            yield return new WaitForSeconds(0.5f);
            isPushed = false;
        }
    }

    public void Shoot()
    {
        if (hpBarInstance.GetCurrentAmmo() > 0)
        {
            hidden = false;
            StartCoroutine(RoFShot());
        }
    }
    public virtual IEnumerator RoFShot()
    {
        hpBarInstance.ReduceAmmo();
        for (int i = 0; i < rateOfFire; i++)
        {
            Vector3 bulletDir = transform.forward.normalized * 2;
            lastShot = Time.time;
            GameObject bulletInstance = Instantiate(ammoPrefab, bulletSpawner.position, Quaternion.identity);
            bulletInstance.transform.parent = null;
            AudioManagerY.Instance.PlayAudio(audioShoot, 1);
            //bulletInstance.transform.Translate(bulletTarget * 100 * Time.deltaTime);
            bulletInstance.GetComponent<Ammo>().SetTargetPos(bulletDir, range);
            yield return new WaitForSeconds(rofDelay);
        }
    }

    public virtual void TakeDamage(float dmgTaken)
    {
        Debug.Log(gameObject.name + " Take damage");
        currentHP = currentHP - dmgTaken;
        hpBarInstance.SetCurrentHP(currentHP);
        Knockdown();
    }

    public virtual void HealDamage(float dmgHeal)
    {
        if(currentHP <= maxHP)
        {
            currentHP = currentHP + dmgHeal;
            hpBarInstance.SetCurrentHP(currentHP);
        }
    }

    public virtual void Knockdown()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        } 
    }

    public FloatingHealthBar GetHealthBar()
    {
        return hpBarInstance;
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

}