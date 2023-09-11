using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{
    [SerializeField] private AudioClip audioKlik;
    private float smoothSpeed = 0.125f;
    int currentPosIndex = 0;
    //kamera sekarang
    bool masukMenu = false;
    Camera mainCamera;
    [SerializeField] private Vector3[] posisiKamera = new Vector3[8];
    //enkapsulasi

    //sebelum start
    private void Awake()
    {
        mainCamera = Camera.main;
        SetPosisiKamera(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!masukMenu && Input.anyKeyDown) //any button
        {
            masukMenu = true;
            GantiPosisiKamera(1);
        }
    }

    private void FixedUpdate()
    {

        Vector3 smoothPos = Vector3.Lerp(mainCamera.transform.position, posisiKamera[currentPosIndex], smoothSpeed);
        mainCamera.transform.position = smoothPos;
    }

    public void SetPosisiKamera(int indexPos)
    {
        mainCamera.transform.position = posisiKamera[indexPos];
        currentPosIndex = indexPos;
    }

    public void GantiPosisiKamera(int indexPos)
    {
        AudioManagerY.Instance.PlayAudio(audioKlik, 1);
            currentPosIndex = indexPos;
    }

}
