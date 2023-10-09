using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour, IDialogueEvent
{
    [SerializeField] private int eventID;
    public string sceneName;

    public int GetEventId()
    {
        return eventID;
    }

    public void StartEvent()
    {
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
