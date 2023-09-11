using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RewardBox : MonoBehaviour
{
    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenuFixY");
    }
}
