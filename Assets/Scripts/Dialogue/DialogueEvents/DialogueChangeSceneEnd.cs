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
        Debug.Log("Loading Scene " + sceneName);
        //SceneManager.LoadScene(sceneName);

        if (FindObjectOfType<SceneLoading>())
        {
            FindObjectOfType<SceneLoading>().LoadScene(sceneName, 0);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
