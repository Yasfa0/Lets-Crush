using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBar : MonoBehaviour
{
    [SerializeField] private Image ammoIcon;
    [SerializeField] private List<Image> slotImg = new List<Image>();
    [SerializeField] private List<Text> cdText = new List<Text>();
    [SerializeField] private Sprite emptySlotImg;
    [SerializeField] private Text ammoCount;

    public void SetAmmoCount(string ammo)
    {
        ammoCount.text = ammo.ToString();
    }

    public void SetCDText(int index,string time)
    {
        cdText[index].text = time.ToString();
    }

    public void SetAmmoIcon(Sprite icon)
    {
        ammoIcon.sprite = icon;
    }

    public void SetSlotImg(int index, Sprite icon)
    {
        slotImg[index].sprite = icon;
    }

    public void EraseSlotImg(int index)
    {
        slotImg[index].sprite = emptySlotImg;
    }
}
