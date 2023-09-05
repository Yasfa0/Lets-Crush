using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WIPBox : MonoBehaviour
{
    [SerializeField] private Text WIPText;
    private string WIPString;
    
    public void CloseWIP()
    {
        FindObjectOfType<MainMenuManager>().SetWIPShown(false);
        Destroy(gameObject);
    }

    public void SetButtonName(string buttonName)
    {
        WIPString = WIPText.text;
        WIPString = buttonName + " " + WIPString;

        WIPText.text = WIPString;
    }
}
