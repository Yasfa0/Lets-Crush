using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosiveEffect : MonoBehaviour
{
    [SerializeField] bool isFriendly = true;
    [SerializeField] float duration = 1f;
    [SerializeField] float dps = 25f;

    float startTime;

    private void Awake()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - startTime >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isFriendly && other.gameObject.tag == "Enemy")
        {
            other.GetComponent<CharacterBase>().TakeDamage(dps);
        }else if (!isFriendly && other.gameObject.tag == "Player")
        {
            other.GetComponent<CharacterBase>().TakeDamage(dps);
        }
    }


}
