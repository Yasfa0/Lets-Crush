using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    //[SerializeField] private string campaignScene;
    //[SerializeField] private GameObject WIPBox;
    //bool WIPShown = false;

    //Untuk Singleton
    public static MainMenuManager Instance;

    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private List<CallDialogueSpeaker> dialogueCalls = new List<CallDialogueSpeaker>();

    private StageData selectedStage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (!GameSetting.CheckSetting())
        {
            Debug.Log("Create Temp Setting");
            SettingData tempSetting = new SettingData();
            GameSetting.SaveSetting(tempSetting);
        }

        FindObjectOfType<OptionMenu>().SetupOption();
        Screen.fullScreen = GameSetting.LoadSetting().isFullscreen;
    }

    public void StartCallDialogue(int callIndex)
    {
        dialogueCanvas.SetActive(true);
        dialogueCalls[callIndex].StartDialogue();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCallDialogue(0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveData tempSave = SaveSystem.LoadSave("save");
           
            foreach (AmmoData ammo in tempSave.ammoDatas)
            {
                 ammo.ammoCount += 10;
            }

            SaveSystem.SaveGame(tempSave,"save");
            foreach (AmmoSelector ammoSelector in FindObjectsOfType<AmmoSelector>())
            {
                ammoSelector.UpdateSaveAmmo();
            }
        }
    }

    public void SetSelectedStage(StageData selectedStage)
    {
        this.selectedStage = selectedStage;
    }

    public StageData getSelectedStage()
    {
        return selectedStage;
    }

    public void WIPMessage(string buttonTxt)
    {
        /*if (!WIPShown)
        {
            GameObject Msg = Instantiate(WIPBox, transform);
            Msg.GetComponent<WIPBox>().SetButtonName(buttonTxt);
            //Msg.transform.SetParent(gameObject.transform);
            WIPShown = true;
        }*/
    }

    public void QuitGame()
    {
        Debug.Log("Quit Application");
        Application.Quit();
    }

    public void SetWIPShown(bool WIPShown)
    {
        //this.WIPShown = WIPShown;
    }
}
