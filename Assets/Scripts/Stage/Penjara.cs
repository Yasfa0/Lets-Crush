using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Penjara : MonoBehaviour
{
    [SerializeField] List<Transform> jailPost = new List<Transform>();

    public void PutInJail(GameObject target)
    {
        int random = Random.Range(0, jailPost.Count);
        target.GetComponent<Collider>().isTrigger = true;
        target.GetComponent<NavMeshAgent>().enabled = false;
        //target.transform.localScale = target.transform.localScale * 0.6f;
        target.transform.SetParent(transform);
        target.transform.localPosition = jailPost[random].localPosition;
        target.GetComponent<CharacterBase>().enabled = false;
    }

}
