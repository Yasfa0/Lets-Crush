using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelector : MonoBehaviour
{
    private int currentIndex = 0;
    [SerializeField] private AudioClip audioKlik;
    [SerializeField] private List<StageData> stageList = new List<StageData>();
    [SerializeField] private List<int> dialogueIDs = new List<int>();
    private bool dialogDone = false;
    [SerializeField] private Image previewImg;
    [SerializeField] private Text descContentTxt;
    [SerializeField] private Text bossContentTxt;

    private void Awake()
    {
        previewImg.sprite = stageList[currentIndex].stagePreview;
        descContentTxt.text = stageList[currentIndex].stageDesc;
        bossContentTxt.text = stageList[currentIndex].bossInfo;
    }

    public void ChangeSummonable(int val)
    {
        currentIndex = currentIndex + val;

        Debug.Log("Current Index " + currentIndex);
        if(currentIndex >= stageList.Count)
        {
            currentIndex = 0;
        }
        else if (currentIndex < 0)
        {
            currentIndex = stageList.Count - 1;
        }

        previewImg.sprite = stageList[currentIndex].stagePreview;
        descContentTxt.text = stageList[currentIndex].stageDesc;
        bossContentTxt.text = stageList[currentIndex].bossInfo;
        AudioManagerY.Instance.PlayAudio(audioKlik, 1);
    }

    public void ConfirmStageSelect(int camPos)
    {
        if (stageList[currentIndex].canBePlayed)
        {
            FindObjectOfType<MainMenuManager>().SetSelectedStage(stageList[currentIndex]);
            FindObjectOfType<MenuCameraController>().GantiPosisiKamera(camPos);
            MainMenuManager.Instance.StartCallDialogue(dialogueIDs[currentIndex]);
            dialogDone = true;
            EventSystem.current.SetSelectedGameObject(null);
            
        }
    }

    public List<StageData> GetStageList()
    {
        return stageList;
    }

    public StageData GetSelectedStage()
    {
        return stageList[currentIndex];
    }
}
