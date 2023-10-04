using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private int followRot = 30;
    [SerializeField] private int focusRot = 0;
    [SerializeField] private GameObject target;
    [SerializeField] private float[] camZLimit = new float[2]; 
    float zOffset;
    float yOffset;
    [SerializeField] private Vector3 bossOffset = new Vector3();
    private bool isFollowing = true;
    private bool isFocusing = false;
    Camera mainCamera;
    private float smoothSpeed = 0.125f;
    Vector3 targetPos = new Vector3();

    private void Awake()
    {
        mainCamera = Camera.main;
        zOffset = target.transform.position.z - transform.position.z;
        yOffset = target.transform.position.y - transform.position.y;
    }

    private void Update()
    {
        if (isFollowing)
        {
            //if(transform.position.z >= camZLimit[0] && transform.position.z <= camZLimit[1])
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, zOffset, Mathf.Clamp(target.transform.position.z - zOffset, camZLimit[0], camZLimit[1])), 1);
            transform.eulerAngles = new Vector3(followRot,0,0);
        }


        if (!isFollowing && isFocusing)
        {
            Vector3 smoothPos = Vector3.Lerp(mainCamera.transform.position, targetPos + bossOffset, smoothSpeed);
            mainCamera.transform.position = smoothPos;
            transform.eulerAngles = new Vector3(focusRot, 0, 0);
        }
    }

    private void FixedUpdate()
    {
    }

    public void FocusCam(Vector3 targetPos,float duration)
    {
        StartCoroutine(WaitFocusCam(targetPos,duration));
    }

    IEnumerator WaitFocusCam(Vector3 targetPos, float duration)
    {
        isFocusing = true;
        isFollowing = false;
        this.targetPos = targetPos;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        isFocusing = false;
        isFollowing = true;
    }

    public void SetIsFollowing(bool isFollowing)
    {
        this.isFollowing = isFollowing;
    }

    public bool GetIsFollowing()
    {
        return isFollowing;
    }

}