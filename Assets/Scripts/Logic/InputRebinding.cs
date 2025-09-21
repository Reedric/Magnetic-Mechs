using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputRebinding : MonoBehaviour {

    public static InputRebinding Instance { get; private set; }

    public event EventHandler OnInputRebinding;

    [SerializeField] InputActionAsset inputActions;

    [Header("Input Actions")]
    [SerializeField] string moveInputActionString;
    [SerializeField] string jumpInputActionString;
    [SerializeField] string fireInputActionString;
    [SerializeField] string launchMagnetInputActionString;
    [SerializeField] string attractInputActionString;
    [SerializeField] string repelInputActionString;
    [SerializeField] string menuInputActionString;

    private const string PLAYER_PREFS_BINDING_OVERRIDES = "Binding Overrides";

    public enum Binding {
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

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("There is more than one InputRebinding " + this);
        }
    }

    private void Start() {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING_OVERRIDES)) {
            string overridesJson = PlayerPrefs.GetString(PLAYER_PREFS_BINDING_OVERRIDES);
            inputActions.LoadBindingOverridesFromJson(overridesJson);
        }
    }

    public void RebindBinding(Binding binding) {
        inputActions.FindActionMap("Player").Disable();

        InputAction action;
        int bindingIndex = 0;
        switch (binding) {
            default:
            case Binding.MOVE_UP:
                action = inputActions.FindAction(moveInputActionString);
                bindingIndex = 1;
                break;
            case Binding.MOVE_LEFT:
                action = inputActions.FindAction(moveInputActionString);
                bindingIndex = 2;
                break;
            case Binding.MOVE_DOWN:
                action = inputActions.FindAction(moveInputActionString);
                bindingIndex = 3;
                break;
            case Binding.MOVE_RIGHT:
                action = inputActions.FindAction(moveInputActionString);
                bindingIndex = 4;
                break;
            case Binding.JUMP:
                action = inputActions.FindAction(jumpInputActionString);
                break;
            case Binding.FIRE:
                action = inputActions.FindAction(fireInputActionString);
                break;
            case Binding.LAUNCH_MAGNET:
                action = inputActions.FindAction(launchMagnetInputActionString);
                break;
            case Binding.ATTRACT:
                action = inputActions.FindAction(attractInputActionString);
                break;
            case Binding.REPEL:
                action = inputActions.FindAction(repelInputActionString);
                break;
            case Binding.MENU:
                action = inputActions.FindAction(menuInputActionString);
                break;
        }

        action.PerformInteractiveRebinding(bindingIndex).OnComplete(callback => {
            inputActions.FindActionMap("Player").Enable();
            PlayerPrefs.SetString(PLAYER_PREFS_BINDING_OVERRIDES, inputActions.SaveBindingOverridesAsJson());
            OnInputRebinding?.Invoke(this, EventArgs.Empty);
        }).Start();
    }

    public void ResetAllBindings() {
        InputActionMap playerActionMap = inputActions.FindActionMap("Player");
        playerActionMap.Disable();
        inputActions.RemoveAllBindingOverrides();
        playerActionMap.Enable();

        PlayerPrefs.SetString(PLAYER_PREFS_BINDING_OVERRIDES, inputActions.SaveBindingOverridesAsJson());
        OnInputRebinding?.Invoke(this, EventArgs.Empty);
    }

    public string GetBinding(Binding binding) {
        switch (binding) {
            default:
            case Binding.MOVE_UP:
                return inputActions.FindAction(moveInputActionString).bindings[1].ToDisplayString();
            case Binding.MOVE_LEFT:
                return inputActions.FindAction(moveInputActionString).bindings[2].ToDisplayString();
            case Binding.MOVE_DOWN:
                return inputActions.FindAction(moveInputActionString).bindings[3].ToDisplayString();
            case Binding.MOVE_RIGHT:
                return inputActions.FindAction(moveInputActionString).bindings[4].ToDisplayString();
            case Binding.JUMP:
                return inputActions.FindAction(jumpInputActionString).bindings[0].ToDisplayString();
            case Binding.FIRE:
                return inputActions.FindAction(fireInputActionString).bindings[0].ToDisplayString();
            case Binding.LAUNCH_MAGNET:
                return inputActions.FindAction(launchMagnetInputActionString).bindings[0].ToDisplayString();
            case Binding.ATTRACT:
                return inputActions.FindAction(attractInputActionString).bindings[0].ToDisplayString();
            case Binding.REPEL:
                return inputActions.FindAction(repelInputActionString).bindings[0].ToDisplayString();
            case Binding.MENU:
                return inputActions.FindAction(menuInputActionString).bindings[0].ToDisplayString();
        }
    }
}
