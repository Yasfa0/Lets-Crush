using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCheck : MonoBehaviour
{
    [SerializeField] private List<AmmoScriptable> playableAmmoList = new List<AmmoScriptable>();

    private void Awake()
    {
        if (!SaveSystem.CheckSave())
        {
            Debug.Log("Create Temp Save");
            SaveData tempSave = new SaveData();

            List<AmmoData> tempAmmos = new List<AmmoData>();
            for (int i = 0; i < playableAmmoList.Count; i++)
            {
                AmmoData tempBullet = new AmmoData();
                tempBullet.CreateFromScriptable(playableAmmoList[i]);
                if (tempBullet.isInfinite)
                {
                    tempBullet.ammoCount = 999;
                }
                tempAmmos.Add(tempBullet);
            }

            tempSave.ammoDatas = tempAmmos;
            SaveSystem.SaveGame(tempSave,"save");
        }
        else
        {
            foreach (AmmoData ammo in SaveSystem.LoadSave("save").ammoDatas)
            {
                Debug.Log("Ammo ID " + ammo.ammoID);
            }
        }
    }

}
