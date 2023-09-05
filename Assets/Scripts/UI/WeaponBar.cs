using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBar : MonoBehaviour
{
    public Image ammoIcon;

    public void SetAmmoIcon(Sprite icon)
    {
        ammoIcon.sprite = icon;
    }

}
