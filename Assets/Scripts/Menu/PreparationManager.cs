using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreparationManager : MonoBehaviour
{
    //[SerializeField] private string startScene;
    [SerializeField] private List<AllySelector> prepSelectors = new List<AllySelector>();
    [SerializeField] private List<AmmoSelector> ammoSelectors = new List<AmmoSelector>();

    public void ConfirmPrep()
    {
        GameData.GetSummonDatas().Clear();
        GameData.GetSelectedAmmo().Clear();
        GameData.GetAmmoCount().Clear();

        for (int i = 0; i < prepSelectors.Count; i++)
        {
            GameData.GetSummonDatas().Add(prepSelectors[i].GetSelectedSummon());
            //Debug.Log("Last Insert : " +GameData.GetSummonDatas()[i]);
        }

        for (int i = 0; i < ammoSelectors.Count; i++)
        {
            GameData.GetSelectedAmmo().Add(ammoSelectors[i].GetSelectedAmmo());
            GameData.GetAmmoCount().Add(ammoSelectors[i].GetSelectedAmmoCount());
        }

        for (int i = 0; i < GameData.GetSummonDatas().Count; i++)
        {
            Debug.Log(i+1 + " : "  + GameData.GetSummonDatas()[i].allyPrefab.name);
        }

        for (int i = 0; i < GameData.GetSelectedAmmo().Count; i++)
        {
            Debug.Log(i + 1 + " : " + GameData.GetSelectedAmmo()[i].ammoPrefab.name);
            Debug.Log(i + 1 + " : " + GameData.GetAmmoCount()[i]);
        }

        //Kurangin Ammo di save data
        SaveData currentSave = SaveSystem.LoadSave("save");

        for (int i = 0; i < ammoSelectors.Count; i++)
        {

            for (int j = 0; j < currentSave.ammoDatas.Count; j++)
            {
                if (currentSave.ammoDatas[j].ammoID == ammoSelectors[i].GetSelectedAmmo().ammoID)
                {
                    currentSave.ammoDatas[j].ammoCount -= ammoSelectors[i].GetSelectedAmmoCount();
                }
            }

        }

        SaveSystem.SaveGame(currentSave, "save");

        SceneManager.LoadScene(FindObjectOfType<MainMenuManager>().getSelectedStage().sceneName);
    }
}