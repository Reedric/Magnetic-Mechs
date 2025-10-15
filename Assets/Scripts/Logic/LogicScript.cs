using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class LogicScript : MonoBehaviour
{
    public enum GameMenuState {
        PLAYING,
        PAUSE_MENU,
        SETTINGS_MENU
    }

    //A singleton intended to hold functions that are used regularly by other scripts
    [Header("Components")]
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    private MultiSceneVariables multiSceneVariables;
    //public Text remainingFuelText;
    public GameObject settingsScreen;
    public PlayerInput playerInput;
    public ButtonSelectionManager buttonSelectionManager;

    [Header("Singleton")]
    public static LogicScript logicSingleton;

    private GameMenuState menuState;

    private void Awake()
    {
        if (logicSingleton == null)
        {
            logicSingleton = this;
        }
        multiSceneVariables = GameObject.FindGameObjectWithTag("MultiSceneVariables").GetComponent<MultiSceneVariables>();
    }
    private void Start()
    {
        buttonSelectionManager.SetGameMenuState(GameMenuState.PLAYING);
    }
    public void TryAgain()
    {
        menuState = GameMenuState.PLAYING;
        Time.timeScale = 1.0f;
        //playerInput.SwitchCurrentActionMap("Player");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartLevelTwo()
    {
        multiSceneVariables.setCheckpoint(0);
        SceneManager.LoadScene("Level 2");
    }
    public void StartLevelThree()
    {
        multiSceneVariables.setCheckpoint(0);
        SceneManager.LoadScene("Level 3");
    }
    public void StartLevelFour()
    {
        multiSceneVariables.setCheckpoint(0);
        SceneManager.LoadScene("Level 4");
    }
    public void StartLevelFive()
    {
        multiSceneVariables.setCheckpoint(0);
        SceneManager.LoadScene("Level 5");
    }
    public void StartLevelSix()
    {
        multiSceneVariables.setCheckpoint(0);
        SceneManager.LoadScene("Level 6");
    }
    public void StartLevelSeven()
    {
        multiSceneVariables.setCheckpoint(0);
        SceneManager.LoadScene("Level 7");
    }
    public void StartLevelEight()
    {
        multiSceneVariables.setCheckpoint(0);
        SceneManager.LoadScene("Level 8");
    }
    public void StartLevelNine()
    {
        multiSceneVariables.setCheckpoint(0);
        SceneManager.LoadScene("Level 9");
    }
    public void StartLevelSelect()
    {
        menuState = GameMenuState.PLAYING;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main Menu");
    }
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
    public void Pause()
    {
        //Debug.Log("Pause from old state = " + menuState);
        switch (menuState) {
            case GameMenuState.PLAYING:
                ShowPauseMenu();
                break;
            case GameMenuState.PAUSE_MENU:
                HideMenus();
                break;
            case GameMenuState.SETTINGS_MENU:
                ShowPauseMenu();
                break;
        }
    }
    public void ShowSettingsMenu()
    {
        // Pause game
        Time.timeScale = 0.0f;
        playerInput.SwitchCurrentActionMap("UI");

        // Show settings menu
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);
        
        // Update button selection visual
        menuState = GameMenuState.SETTINGS_MENU;
        buttonSelectionManager.SetGameMenuState(menuState);

        //Debug.Log("ShowSettingsMenu; new state = " + menuState);
    }
    public void ShowPauseMenu()
    {
        // Pause game
        Time.timeScale = 0.0f;
        playerInput.SwitchCurrentActionMap("UI");

        // Show pause menu
        pauseScreen.SetActive(true);
        settingsScreen.SetActive(false);

        // Update button selection visual
        menuState = GameMenuState.PAUSE_MENU;
        buttonSelectionManager.SetGameMenuState(menuState);

        //Debug.Log("ShowPauseMenu; new state = " + menuState);
    }
    public void HideMenus()
    {
        // Unpause game
        Time.timeScale = 1.0f;
        playerInput.SwitchCurrentActionMap("Player");

        // Hide menus
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(false);
        
        menuState = GameMenuState.PLAYING;

        //Debug.Log("HideMenus; new state = " + menuState);
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
        get { return menuState != GameMenuState.PLAYING; }
    }
    public void StartPostSpiderBossDelay()
    {
        StartCoroutine(StartPostSpiderBoss(3f));
    }
    public IEnumerator StartPostSpiderBoss(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartLevelFive();
    }
    public void StartPostBeeBossDelay()
    {
        StartCoroutine(StartPostBeeBoss(3f));
    }
    public IEnumerator StartPostBeeBoss(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartLevelNine();
    }
}
