using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseBox : MonoBehaviour
{
    public void ToMenu()
    {
        Weapon weap = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapon>();
        weap.SaveAmmoData();
        FindObjectOfType<ObjectiveManager>().TogglePause(false);
        SceneManager.LoadScene("MainMenuY2");
    }
}
