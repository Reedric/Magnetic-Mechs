using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InputRebinding : MonoBehaviour
{

    public static InputRebinding Instance { get; private set; }

    [SerializeField] private InputActionAsset playerInputAsset;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button fireButton;
    [SerializeField] private Button launchMagnetButton;
    [SerializeField] private Button attractButton;
    [SerializeField] private Button repelButton;
    [SerializeField] private Button menuButton;
    private const string PLAYER_PREFS_BINDING_OVERRIDES = "Binding Overrides";

    public enum Binding
    {
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT,
        JUMP,
        FIRE,
        LAUNCH_MAGNET,
        ATTRACT,
        REPEL,
        MENU
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING_OVERRIDES))
        {
            string overridesJson = PlayerPrefs.GetString(PLAYER_PREFS_BINDING_OVERRIDES);
            playerInputAsset.LoadBindingOverridesFromJson(overridesJson);
        }
    }

    public void RebindBinding(Binding binding)
    {

    }

    public string GetBinding(Binding binding)
    {
        return "empty";
    }

}
