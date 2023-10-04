using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CharacterBase>())
        {
            other.GetComponent<CharacterBase>().SetHidden(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterBase>())
        {
            if (other.GetComponent<CharacterBase>().GetHidden())
            {
                other.GetComponent<CharacterBase>().SetHidden(false);
            }
        }
    }
}
