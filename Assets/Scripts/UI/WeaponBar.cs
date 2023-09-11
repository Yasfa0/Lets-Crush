using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBar : MonoBehaviour
{
    [SerializeField] private Image ammoIcon;
    [SerializeField] private List<Image> slotImg = new List<Image>();
    [SerializeField] private Sprite emptySlotImg;

    private void Start()
    {
        
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
