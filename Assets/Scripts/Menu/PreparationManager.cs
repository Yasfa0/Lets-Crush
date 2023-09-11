using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreparationManager : MonoBehaviour
{
    //[SerializeField] private string startScene;
    [SerializeField] private List<AllySelector> prepSelectors = new List<AllySelector>();

    public void ConfirmPrep()
    {
        GameData.GetSummonDatas().Clear();
        for (int i = 0; i < prepSelectors.Count; i++)
        {
            GameData.GetSummonDatas().Add(prepSelectors[i].GetSelectedSummon());
            //Debug.Log("Last Insert : " +GameData.GetSummonDatas()[i]);
        }

        for (int i = 0; i < GameData.GetSummonDatas().Count; i++)
        {
            Debug.Log(i+1 + " : "  + GameData.GetSummonDatas()[i].allyPrefab.name);
        }

        SceneManager.LoadScene(FindObjectOfType<MainMenuManager>().getSelectedStage().sceneName);
    }
}
