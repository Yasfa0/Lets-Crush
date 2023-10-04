using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoSelector : MonoBehaviour
{
    private int currentIndex = 0;
    [SerializeField] private AudioClip audioKlik;
    [SerializeField] private Image iconImg;
    [SerializeField] private List<AmmoScriptable> ammoList = new List<AmmoScriptable>();
    private List<int> ammoCount = new List<int>();
    [SerializeField] private Slider ammoSlider;
    [SerializeField] private Text ammoCountText;
    SaveData currentSave;

    private void Start()
    {
        currentSave = SaveSystem.LoadSave("save");

        foreach (AmmoScriptable ammo in ammoList)
        {
            foreach (AmmoData saveAmmo in currentSave.ammoDatas)
            {
                if(ammo.ammoID == saveAmmo.ammoID)
                {
                    ammoCount.Add(saveAmmo.ammoCount);
                }
            }
        }

        UpdateSelector();
    }

    public void UpdateSaveAmmo()
    {
        currentSave = SaveSystem.LoadSave("save");

        ammoCount.Clear();
        foreach (AmmoScriptable ammo in ammoList)
        {
            foreach (AmmoData saveAmmo in currentSave.ammoDatas)
            {
                if (ammo.ammoID == saveAmmo.ammoID)
                {
                    ammoCount.Add(saveAmmo.ammoCount);
                }
            }
        }

        UpdateSelector();
    }

    public void UpdateSelector()
    {
        iconImg.sprite = ammoList[currentIndex].ammoIcon;
        ammoSlider.maxValue = ammoCount[currentIndex];
        if (ammoSlider.value > ammoCount[currentIndex])
        {
            ammoSlider.value = ammoCount[currentIndex];
        }
        SliderCounter();
    }

    public void SliderCounter()
    {
        ammoCountText.text = Mathf.FloorToInt(ammoSlider.value).ToString();
    }

    public void ChangeAmmo(int val)
    {
        currentIndex = currentIndex + val;

        Debug.Log("Current Index " + currentIndex);
        if (currentIndex >= ammoList.Count)
        {
            currentIndex = 0;
        }
        else if (currentIndex < 0)
        {
            currentIndex = ammoList.Count - 1;
        }

        AudioManagerY.Instance.PlayAudio(audioKlik, 1);
        UpdateSelector();
    }

    public List<AmmoScriptable> GetAmmoList()
    {
        return ammoList;
    }

    public AmmoScriptable GetSelectedAmmo()
    {
        return ammoList[currentIndex];
    }

    public int GetSelectedAmmoCount()
    {
        return ammoCount[currentIndex];
    }

    public void SetAmmoCount(List<int> ammo)
    {
        ammoCount = ammo;
    }

}