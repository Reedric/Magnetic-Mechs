using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsLogicScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Controls
    public KeyCode rightMove = KeyCode.D;
    public KeyCode leftMove = KeyCode.A;
    public KeyCode drop = KeyCode.S;
    public KeyCode jump = KeyCode.Space;
    public KeyCode attract = KeyCode.LeftControl;
    public KeyCode repel = KeyCode.LeftShift;
    public KeyCode wFire = KeyCode.Mouse0;
    public KeyCode mFire = KeyCode.Mouse1;
    public KeyCode charge = KeyCode.F;
    //Functions

    public KeyCode buttonToKeybind(Button ogButton)
    {
        if (ogButton.name == "RightMove")
        {
            return rightMove;
        }
        if (ogButton.name == "LeftMove")
        {
            return leftMove;
        }
        if (ogButton.name == "DropDown")
        {
            return drop;
        }
        if (ogButton.name == "Thrusters")
        {
            return jump;
        }
        if (ogButton.name == "Attract")
        {
            return attract;
        }
        if (ogButton.name == "Repel")
        {
            return repel;
        }
        if (ogButton.name == "ShootWeapon")
        {
            return wFire;
        }
        if (ogButton.name == "ShootMagnet")
        {
            return mFire;
        }
        if (ogButton.name == "Charge")
        {
            return charge;
        }
        else
        {
            return KeyCode.F1;
        }
    }
    public void toGoBack()
    {
        LogicScript.logicSingleton.goBack();
    }

    public void toChangeBind()
    {
        //LogicScript.logicSingleton.changeBind(gameObject);
        
    }
}
