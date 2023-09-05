using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChecker : MonoBehaviour
{
    [SerializeField] private string checkTag;
    private List<GameObject> detectedCharacter = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == checkTag)
        {
            bool newAddition = true;

            foreach (GameObject obj in detectedCharacter)
            {
                if (obj == other.gameObject)
                {
                    newAddition = false;
                }
            }

            if (newAddition)
            {
                detectedCharacter.Add(other.gameObject);
                Debug.Log(other.name + " entered a base area");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == checkTag)
        {

            for (int i = 0; i < detectedCharacter.Count; i++)
            {
                if (detectedCharacter[i] == other.gameObject)
                {
                    detectedCharacter.RemoveAt(i);
                    Debug.Log(other.name + " left a base area");
                }
            }

        }
    }

    public List<GameObject> GetDetectedCharaList()
    {
        return detectedCharacter;
    }

}
