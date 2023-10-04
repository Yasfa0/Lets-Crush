using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    [SerializeField] AmmoScriptable ammoItem;
    [SerializeField] int ammoAmount = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Weapon>())
        {
            other.GetComponent<Weapon>().AddNewAmmo(ammoItem, ammoAmount);
            Destroy(gameObject);
        }
    }
}