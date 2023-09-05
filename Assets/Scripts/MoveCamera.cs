using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransformation : MonoBehaviour
{
    public Vector3 newPosition = new Vector3(0, 14, -171);

    void Start()
    {
        transform.position = newPosition;
    }
}
