using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreenLogic : MonoBehaviour
{
    public GameObject startScreenStart;
    public GameObject startScreenLevelSelect;
    public MultiSceneVariables variableStorage;
    public PlayerInput myInput;
    public PlayerInput selectionInput;

    private void Awake()
    {
        myInput = GetComponent<PlayerInput>();
    }
    public void StartGame()
    {
        startScreenStart.SetActive(false);
        startScreenLevelSelect.SetActive(true);
    }
    public void StartStage1()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void StartStage2()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void StartStage3()
    {
        SceneManager.LoadScene("Level 3");
    }
    public void StartStage4()
    {
        SceneManager.LoadScene("Level 4");
    }
    public void GamePadPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            variableStorage.gamePadNotMouse = true;
            myInput.SwitchCurrentActionMap("UI");
            StartGame();
        }
    }
    public void SpacePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            variableStorage.gamePadNotMouse = false;
            myInput.SwitchCurrentActionMap("UI");
            StartGame();
        }
    }
}
