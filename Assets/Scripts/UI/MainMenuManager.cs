using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string campaignScene;
    [SerializeField] private GameObject WIPBox;

    bool WIPShown = false;

    public void StartCampaign()
    {
        SceneManager.LoadScene(campaignScene);
    }

    public void WIPMessage(string buttonTxt)
    {
        if (!WIPShown)
        {
            GameObject Msg = Instantiate(WIPBox, transform);
            Msg.GetComponent<WIPBox>().SetButtonName(buttonTxt);
            //Msg.transform.SetParent(gameObject.transform);
            WIPShown = true;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Application");
        Application.Quit();
    }

    public void SetWIPShown(bool WIPShown)
    {
        this.WIPShown = WIPShown;
    }
}
