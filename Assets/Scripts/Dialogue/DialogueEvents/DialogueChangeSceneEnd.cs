using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueChangeSceneEnd : MonoBehaviour, IDialogueEvent
{
    [SerializeField] private int eventID;
    [SerializeField] private string sceneName;

    public int GetEventId()
    {
        return eventID;
    }

    public void StartEvent()
    {
        SceneManager.LoadScene(sceneName);
        //FindObjectOfType<SceneLoading>().LoadScene(sceneName, 0);
    }
}
