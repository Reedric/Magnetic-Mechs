using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ButtonSelectionManager : MonoBehaviour
{
    [Header("Game Objects")]
    public Transform ButtonParent;
    [Header("Variables")]
    public List<GameObject> Buttons;
    [Header("Timers")]
    private float delay = .02f;
    private float readyToChange = 0f;
    public int CurrentSelection = 0;
    private void Awake()
    {
        foreach (Transform child in ButtonParent)
        {
            GameObject button = child.gameObject;
            Buttons.Add(button);
        }
        GameObject savedVariablesObject = GameObject.FindGameObjectWithTag("MultiSceneVariables");
        setButtonSize(CurrentSelection);
    }
    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log("test");
        float change = context.ReadValue<Vector2>().x;
        if(Time.realtimeSinceStartup > readyToChange)
        {
            if(change > .25)
            {
                CurrentSelection += 1;
                if(CurrentSelection >= Buttons.Count)
                {
                    CurrentSelection = 0;
                }
            }
            else if (change < -.25)
            {
                CurrentSelection -= 1;
                if (CurrentSelection < 0)
                {
                    CurrentSelection = Buttons.Count - 1;
                }
            }
            readyToChange = Time.realtimeSinceStartup + delay;
            setButtonSize(CurrentSelection);
        }
    }
    public void setButtonSize(int currentSelection)
    {
        foreach(GameObject button in Buttons) 
        {
            button.GetComponent<RectTransform>().localScale = Vector3.one;
        }
        Buttons[currentSelection].GetComponent<RectTransform>().localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }
    public void Select(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Buttons[CurrentSelection].GetComponent<Button>().onClick.Invoke();
        }
    }
}
