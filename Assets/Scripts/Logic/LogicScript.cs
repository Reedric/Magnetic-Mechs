using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class LogicScript : MonoBehaviour
{
    //A singleton intended to hold functions that are used regularly by other scripts
    [Header("Components")]
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    //public Text remainingFuelText;
    public GameObject settingsScreen;
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
    public void StartLevelFive()
    {
        SceneManager.LoadScene("Level 5");
    }
    public void StartLevelSix()
    {
        SceneManager.LoadScene("Level 6");
    }
    public void StartLevelSeven()
    {
        SceneManager.LoadScene("Level 7");
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
            settingsScreen.SetActive(false);
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
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);
        Debug.Log("test");
    }
    public void goBack()
    {
        settingsScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }
    /*
    public void changeBind(GameObject button)
    {

    }
    */
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
