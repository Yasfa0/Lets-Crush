using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private AudioClip audioKlik;
    [SerializeField] private AudioClip audioShoot;
    [SerializeField] Transform bulletSpawner;
    [SerializeField] GameObject indicator;
    [SerializeField] private AmmoScriptable defaultAmmo;
    [SerializeField] List<AmmoScriptable> ammoList = new List<AmmoScriptable>();
    private List<int> ammoCount = new List<int>();
    [SerializeField] int rateOfFire = 1;
    [SerializeField] float range = 4f;
    [SerializeField] int ammoCapacity = 2;
    [SerializeField] float regenCD = 2f;
    private int currentAmmoIndex = 0;
    float cooldown = 0.5f;
    float lastShot;
    float rofDelay = 0.3f;
    bool isAiming = false;
    
    FloatingHealthBar statsBar;

    private void Start()
    {
        if (GameData.GetSelectedAmmo().Count > 0)
        {
            ammoList.Clear();
            ammoList.Add(defaultAmmo);
            //ammoList = GameData.GetSelectedAmmo();
            foreach (AmmoScriptable temp in GameData.GetSelectedAmmo())
            {
                ammoList.Add(temp);
            }

            ammoCount.Clear();
            ammoCount.Add(9999);
            foreach (int temp in GameData.GetAmmoCount())
            {
                ammoCount.Add(temp);
            }
        }
        else
        {
            foreach (AmmoScriptable ammo in ammoList)
            {
                ammoCount.Add(10);
            }
        }

        if (ammoList[currentAmmoIndex].isInfinite)
        {
            FindObjectOfType<WeaponBar>().SetAmmoCount("~");
        }
        else
        {
            FindObjectOfType<WeaponBar>().SetAmmoCount(ammoCount[currentAmmoIndex].ToString());
        }

        FindObjectOfType<WeaponBar>().SetAmmoIcon(ammoList[currentAmmoIndex].ammoIcon);
        statsBar = GetComponent<CharacterBase>().GetHealthBar();
        statsBar.SetupAmmoBar(ammoCapacity,regenCD);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeAmmo(1);
        }else if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeAmmo(-1);
        }

        if (isAiming)
        {
            RaycastHit info;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out info, 100))
            {
                //Rotate ke arah mouse
                /*Quaternion lookTarget = Quaternion.LookRotation(info.point - transform.position);
                lookTarget.x = transform.rotation.x;
                lookTarget.z = transform.rotation.z;
                transform.rotation = Quaternion.Lerp(transform.rotation, lookTarget, 60f * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0,transform.rotation.y,0);*/
                Vector3 newtarget = info.point;
                newtarget.y = transform.position.y;
                transform.LookAt(newtarget);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if(ClosestEnemyAvailable() != null && !isAiming)
            {
                GameObject closestEn = ClosestEnemyAvailable();
                //float angle = Vector3.Angle(transform.forward, closestEn.transform.position);
                //transform.LookAt(closestEn.transform);
                Quaternion lookTarget = Quaternion.LookRotation(closestEn.transform.position - transform.position);
                //transform.rotation = Quaternion.Slerp(transform.rotation, lookTarget, 50f * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookTarget, 60f * Time.deltaTime);
                float angleLeft = Vector3.Angle(transform.forward, closestEn.transform.position - transform.position);
                Debug.Log("Angle Left " + angleLeft);
                if (Time.time - lastShot > cooldown && angleLeft < 10)
                {
                    Shoot();
                }
            }else if (Time.time - lastShot > cooldown)
            {
                Shoot();
            }    
        }
        
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
            indicator.SetActive(isAiming);
        }
        else
        {
            isAiming = false;
            indicator.SetActive(isAiming);
        }
    }

    void Shoot()
    {
        if (statsBar.GetCurrentAmmo() > 0)
        {
            if (ammoCount[currentAmmoIndex] > 0)
            {
                GetComponent<CharacterBase>().SetHidden(false);
                StartCoroutine(RoFShot());
            }
        }
    }

    public void AddNewAmmo(AmmoScriptable newAmmo, int amount)
    {
        //Kalau dah ada jenis ammo yang sama, tambahin jumlah ammo itu
        //Kalau nggak ada, tambahin ammo baru dengan jumlah ammo baru

        bool alreadyAvailable = false;
        for (int i = 0; i < ammoList.Count; i++)
        {
            if (ammoList[i].ammoID == newAmmo.ammoID)
            {
                ammoCount[i] += amount;
                alreadyAvailable = true;

                if(ammoList[i].ammoID == ammoList[currentAmmoIndex].ammoID)
                {
                    FindObjectOfType<WeaponBar>().SetAmmoCount(ammoCount[i].ToString());
                }
            }
        }

        if (alreadyAvailable)
        {
            ammoList.Add(newAmmo);
            ammoCount.Add(amount);
        }
    }

    public void ChangeAmmo(int changeValue)
    {
        currentAmmoIndex += changeValue;
        AudioManagerY.Instance.PlayAudio(audioKlik, 1);

        if(currentAmmoIndex >= ammoList.Count)
        {
            currentAmmoIndex = 0;
        }
        if (currentAmmoIndex <= -1)
        {
            currentAmmoIndex = ammoList.Count-1;
        }

        Debug.Log("Current Ammo Index: " + currentAmmoIndex);
        if (ammoList[currentAmmoIndex].isInfinite)
        {
            FindObjectOfType<WeaponBar>().SetAmmoCount("~");
        }
        else
        {
            FindObjectOfType<WeaponBar>().SetAmmoCount(ammoCount[currentAmmoIndex].ToString());
        }
        FindObjectOfType<WeaponBar>().SetAmmoIcon(ammoList[currentAmmoIndex].ammoIcon);
        //FindObjectOfType<WeaponBar>().SetAmmoCount(ammoCount[currentAmmoIndex].ToString());
    }

    IEnumerator RoFShot()
    {
        statsBar.ReduceAmmo();
        for (int i = 0; i < rateOfFire; i++)
        {
            Vector3 bulletDir = transform.forward.normalized * 2;
            lastShot = Time.time;
            GameObject bulletInstance = Instantiate(ammoList[currentAmmoIndex].ammoPrefab, bulletSpawner.position, Quaternion.identity);
            bulletInstance.transform.parent = null;
            Debug.Log("Instantiated Bullet " + bulletInstance.name);
            AudioManagerY.Instance.PlayAudio(audioShoot,1);
            //bulletInstance.transform.Translate(bulletTarget * 100 * Time.deltaTime);
            bulletInstance.GetComponent<Ammo>().SetTargetPos(bulletDir, range);
            if(ammoCount.Count > 0 && !ammoList[currentAmmoIndex].isInfinite)
            {
                ammoCount[currentAmmoIndex] -= 1;
                FindObjectOfType<WeaponBar>().SetAmmoCount(ammoCount[currentAmmoIndex].ToString());
            }
            yield return new WaitForSeconds(rofDelay);
        }
    }

    GameObject ClosestEnemyAvailable()
    {
        //Array enemy yang ada dalem range
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> inRange = new List<GameObject>();

        foreach (GameObject en in allEnemy)
        {
            if(Vector3.Distance(en.transform.position,transform.position) <= range * 2)
            {
                inRange.Add(en);
                Debug.Log("In Range");
            }
        }

        //Ambil enemy yang paling deket dari list in range
        GameObject closestEnemy = null;
        float closestDist = Mathf.Infinity;

        foreach (GameObject en in inRange)
        {
            if(Vector3.Distance(en.transform.position, transform.position) < closestDist)
            {
                closestDist = Vector3.Distance(en.transform.position, transform.position);
                closestEnemy = en;
            }
        }
        return closestEnemy;
    }

    public bool GetIsAiming()
    {
        return isAiming;
    }

    public void SaveAmmoData()
    {
        SaveData currentSave = SaveSystem.LoadSave("save");

        for (int i = 0; i < ammoList.Count; i++)
        {
            bool alreadyAvailable = false;
            for (int j = 0; j < currentSave.ammoDatas.Count; j++)
            {
                if(currentSave.ammoDatas[j].ammoID == ammoList[i].ammoID)
                {
                    currentSave.ammoDatas[j].ammoCount += ammoCount[i];
                    alreadyAvailable = true;
                }
            }

            if (!alreadyAvailable)
            {
                AmmoData temp = new AmmoData();
                temp.CreateFromScriptable(ammoList[i]);
                currentSave.ammoDatas.Add(temp);
            }

        }
        SaveSystem.SaveGame(currentSave, "save");
    }

}