using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float[] camZLimit = new float[2]; 
    float zOffset;

    private void Awake()
    {
        zOffset = target.transform.position.z - transform.position.z;
    }

    private void Update()
    {
        //if(transform.position.z >= camZLimit[0] && transform.position.z <= camZLimit[1])
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(target.transform.position.z - zOffset,camZLimit[0],camZLimit[1]) ), 1);
    }

}
