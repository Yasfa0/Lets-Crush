using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Benteng : MonoBehaviour
{
    [SerializeField] private float maxHP = 1000f;
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] private Vector3 hpbarOffset = new Vector3(0,4f,3.5f);
    private FloatingHealthBar hpBarInstance;
    private float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        hpBarInstance = Instantiate(hpBarPrefab, transform.position + hpbarOffset, Quaternion.Euler(70,0,0), null).GetComponent<FloatingHealthBar>();
        hpBarInstance.SetupHealthBar(gameObject, maxHP, currentHP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(float dmgTaken)
    {
        currentHP = currentHP - dmgTaken;
        hpBarInstance.SetCurrentHP(currentHP);

        if(currentHP <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
