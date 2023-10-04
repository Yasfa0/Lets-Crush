using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public void SpawnItem(GameObject itemPrefab)
    {
        GameObject item = Instantiate(itemPrefab, gameObject.transform,true);
        item.transform.parent = null;
        item.transform.position = new Vector3(transform.position.x,item.transform.position.y,transform.position.z);
    }
}
