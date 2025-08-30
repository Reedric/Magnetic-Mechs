using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPromptScript : MonoBehaviour
{
    //private bool hasNotTriggered;
    private Prompt currentPrompt;
    [Header("Public Variables")]
    public string promptScript;
    public float directionalArrowOrientation;
    [Header("Components")]
    private PromptUIScript promptUIScript;
    void Start()
    {
        GameObject promptUI = GameObject.FindGameObjectWithTag("PromptText");
        if (promptUI != null) promptUIScript = promptUI.GetComponent<PromptUIScript>();
        else Debug.Log("Can't find PromptText");
        GameObject allPrompts = GameObject.FindGameObjectWithTag("AllPrompts");
        if (allPrompts != null) currentPrompt = allPrompts.GetComponent<TutorialAllPrompts>().prompts[promptScript];
        else Debug.Log("Can't find PromptText");

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            playPrompt();
        }
    }
    public void playPromptIndefinitely()
    {
        playPrompt();
        promptUIScript.playIndefinitely = true;
    }
    public void playPrompt()
    {
        promptUIScript.updateCurrentPrompt(currentPrompt, directionalArrowOrientation);
    }
}
