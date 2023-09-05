using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private List<Text> healthTexts = new List<Text>();
    Slider healthBar;
    Slider ammoBar;
    float zOffset;
    float ammoCost;
    int maxAmmoCap;
    int currentAmmoCap;
    float regenCD;

    private void Awake()
    {
        healthBar = gameObject.transform.GetChild(0).GetComponent<Slider>();
        ammoBar = gameObject.transform.GetChild(1).GetComponent<Slider>();
        ammoBar.gameObject.SetActive(false);
        CalculateOffset();
    }

    private void Update()
    {
        if(currentAmmoCap < maxAmmoCap)
        {
            ammoBar.value += (ammoCost * Time.deltaTime) / regenCD;
        }

        currentAmmoCap = Mathf.FloorToInt(ammoBar.value / ammoCost);

        //if(transform.position.z >= camZLimit[0] && transform.position.z <= camZLimit[1])
        if(target)
            transform.position = Vector3.Slerp(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z - zOffset), 1);
    }

    public void SetupHealthBar(GameObject t, float maxHP, float currentHP)
    {
        target = t;
        healthBar.maxValue = maxHP;
        healthBar.value = currentHP;
        UpdateHPText();
        CalculateOffset();
    }

    public void SetupAmmoBar(int capacity,float regenCD)
    {
        ammoBar.gameObject.SetActive(true);
        maxAmmoCap = capacity;
        currentAmmoCap = maxAmmoCap;
        ammoBar.maxValue = 100;
        ammoBar.value = 100;
        ammoCost = 100 / capacity;
        this.regenCD = regenCD;
    }

    public void SetCurrentHP(float currentHP)
    {
        healthBar.value = currentHP;
        UpdateHPText();
    }

    private void UpdateHPText()
    {
        foreach (Text hpText in healthTexts)
        {
            hpText.text = ((int)healthBar.value).ToString();
        }
    }

    void CalculateOffset() 
    {
        if(target != null)
            zOffset = target.transform.position.z - transform.position.z;
    }

    public void DestroyHealthBar()
    {
        Destroy(gameObject);
    }

    public float GetAmmoCost()
    {
        return ammoCost;
    }

    public float GetCurrentAmmo()
    {
        return currentAmmoCap;
    }

    public void ReduceAmmo()
    {
        ammoBar.value -= ammoCost;
    }

}
