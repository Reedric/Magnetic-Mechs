using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseLogicScript : MonoBehaviour
{
    //holds all of the functions used for the pause screen
    public void TryAgain()
    {
        LogicScript.logicSingleton.TryAgain();
    }
    public void Pause()
    {
        LogicScript.logicSingleton.Pause();
    }
    public void Menu()
    {
        LogicScript.logicSingleton.Menu();
    }
    public void SetControls()
    {
        LogicScript.logicSingleton.SetControls();
    }
    public void Quit()
    {
        LogicScript.logicSingleton.Quit();
    }
}
