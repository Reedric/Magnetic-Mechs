using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LogicScript : MonoBehaviour
{
    [Header("Components")]
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    //public Text remainingFuelText;
    public PlayerInput playerInput;
    [Header("variables")]
    private bool isPaused = false;
    [Header("Singleton")]
    public static LogicScript logicSingleton;
    private void Awake()
    {
        if (logicSingleton == null)
        {
            logicSingleton = this;
        }
    }
    public void TryAgain()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        //playerInput.SwitchCurrentActionMap("Player");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartLevelTwo()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void StartLevelThree()
    {
        SceneManager.LoadScene("Level 3");
    }
    public void StartLevelFour()
    {
        SceneManager.LoadScene("Level 4");
    }
    public void Menu()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main Menu");
    }
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1.0f;
            playerInput.SwitchCurrentActionMap("Player");
            pauseScreen.SetActive(false);
        }
        else
        {
            Time.timeScale = 0.0f;
            pauseScreen.SetActive(true);
            playerInput.SwitchCurrentActionMap("UI");
        }
        isPaused = !isPaused;
    }
    public void SetControls()
    {
        Debug.Log("test");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public bool IsPaused
    {
        get { return isPaused; }
    }
    public void StartStage4Delay()
    {
        StartCoroutine(StartStage4(3f));
    }
    public IEnumerator StartStage4(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartLevelFour();
    }
}
