using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Prompt : MonoBehaviour
{
    //holds the class Prompt which has two associated variables and a functions for setting them
    public string promptText;
    public float priority; //the higher the number the higher the priority
    public void postHocConstructor(string promptText, float priority)
    {
        this.promptText = promptText;
        this.priority = priority;
    }
}
