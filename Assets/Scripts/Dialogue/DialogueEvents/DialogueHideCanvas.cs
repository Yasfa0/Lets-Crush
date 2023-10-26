using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHideCanvas : MonoBehaviour, IDialogueEvent
{
    [SerializeField] private int eventID;

    public int GetEventId()
    {
        return eventID;
    }

    public void StartEvent()
    {
        Debug.Log("Called");
        DialogueController.Instance.gameObject.SetActive(false);
    }
}
