using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] string mainMenuScene = "MainMenuY2";
    [SerializeField] GameObject pauseMenu;

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            FindObjectOfType<PlayerMovement>().SetPauseControl(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        FindObjectOfType<PlayerMovement>().SetPauseControl(false);
        pauseMenu.SetActive(false);
    }

    public void RestartStage()
    {
        Time.timeScale = 1;
        FindObjectOfType<SceneLoading>().LoadScene(SceneManager.GetActiveScene().name, 0);
    }

    public void ToMainMenu()
    {
        Weapon weap = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapon>();
        weap.SaveAmmoData();
        Time.timeScale = 1;
        FindObjectOfType<SceneLoading>().LoadScene(mainMenuScene,0);
    }
}
