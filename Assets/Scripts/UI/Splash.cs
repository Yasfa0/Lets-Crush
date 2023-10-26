using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    [SerializeField] private int dialogueID;
    static bool alreadyCloses = false;

    private void Awake()
    {
        if (alreadyCloses)
        {
            Destroy(gameObject);
        }
    }

    public void CloseSplash()
    {
        alreadyCloses = true;
        gameObject.SetActive(false);
        MainMenuManager.Instance.StartCallDialogue(dialogueID);
    }
}
