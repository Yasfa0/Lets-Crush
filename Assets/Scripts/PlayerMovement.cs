using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    List<GameObject> captureFollow = new List<GameObject>();
    float horizontalInput, verticalInput;
    Vector3 moveDir;
    [SerializeField] private float speed = 10f;
    CharacterController charaController;
    float turnSmooth;
    Weapon playerWeapon;

    private void Awake()
    {
        playerWeapon = GetComponent<Weapon>();
        charaController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        moveDir = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if(moveDir.magnitude >= 0.1f)
        {
            if (!playerWeapon.GetIsAiming())
            {
                float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, 0.1f);
                transform.rotation = Quaternion.Euler(transform.rotation.x, smoothAngle, transform.rotation.z);
            }
            

            //transform.Translate(moveDir * speed * Time.deltaTime);
            //rb.velocity = moveDir * speed * Time.deltaTime;
            charaController.Move(moveDir * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] hitCollider = Physics.OverlapSphere(gameObject.transform.position, 2f);
           foreach (Collider collider in hitCollider)
            {
                if (collider.GetComponent<Enemy>() != null)
                {
                    if (collider.GetComponent<Enemy>().GetCurrentState().GetCurrentSTATE() == State.STATE.Knockdown)
                    {
                        //collider.transform.SetParent(gameObject.transform);
                        //collider.transform.position = new Vector3(0,0,0);
                        collider.GetComponent<Enemy>().Captured(transform);
                        captureFollow.Add(collider.gameObject);
                    }
                }

                if(captureFollow.Count > 0 && collider.gameObject.tag == "Penjara")
                {
                    foreach (GameObject cap in captureFollow)
                    {
                        cap.GetComponent<Enemy>().Imprisoned();
                        cap.GetComponent<CharacterBase>().GetHealthBar().DestroyHealthBar();
                        Destroy(cap);
                        //cap.transform.position = new Vector3(-999,-999,-999);
                    }
                    MapManager.Instance.FillTeams();
                    captureFollow.Clear();
                    //MapManager.Instance.FillTeams();
                }
            }
        }

    }

}
