using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEndPause : MonoBehaviour, IDialogueEvent
{
    [SerializeField] private int eventID;
    [SerializeField] private bool pauseVal;

    public int GetEventId()
    {
        return eventID;
    }

    public void StartEvent()
    {
        Debug.Log("Unpase End");
        ObjectiveManager.Instance.TogglePause(pauseVal);
    }
}
