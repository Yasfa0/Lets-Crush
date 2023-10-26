using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallDialogueSpeaker : DialogueSpeaker
{
    [SerializeField] private Sprite bgImg;
    public void StartDialogue()
    {
        Debug.Log("Dialogue CHECK: " + DialogueController.Instance.name);
            DialogueController.Instance.SetCurrentSpeaker(this);
            DialogueController.Instance.SetupDialogue(dialogueList);
            DialogueController.Instance.SetBG(bgImg);
            Debug.Log("Speak");    
    }

}
