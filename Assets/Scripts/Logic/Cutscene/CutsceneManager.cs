using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneManager : MonoBehaviour
{
    //code for skipping cutscenes when the player hits space
    [Header("Components")]
    public CutsceneScript currentCutsceneScript;

    public void SkipCutscene()
    {
        if (currentCutsceneScript!=null)
        {
            currentCutsceneScript.SkipCutscene();
        }
    }
}
