using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Controls;

public class ConManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _drop;
    private InputActionMap _currentPlayerMap;
    [SerializeField]
    private InputActionAsset _asset;
    
    [SerializeField]
    private TMP_Text _labelBindUp, _labelBindDown, _labelBindLeft, _labelBindRight, _labelBindSpace;

    [SerializeField]
    List<PlayerInput> _players = new List<PlayerInput>();

    void Start() {
        _currentPlayerMap = _asset.FindActionMap("Player1");
        RedrawUI();
        _drop.onValueChanged.AddListener(OnPlayerChanged);
    }
    void Update() {

    }
    void OnPlayerChanged(int selectedValue) {
        _currentPlayerMap = _asset.FindActionMap(_drop.options[selectedValue].text);
        RedrawUI();
    }
    void RedrawUI() {
        _labelBindUp.text = _currentPlayerMap.FindAction("Move").bindings[2].ToDisplayString();
        _labelBindDown.text = _currentPlayerMap.FindAction("Move").bindings[1].ToDisplayString();

        _labelBindLeft.text = _currentPlayerMap.FindAction("Rotate").bindings[1].ToDisplayString();
        _labelBindRight.text = _currentPlayerMap.FindAction("Rotate").bindings[2].ToDisplayString();

        _labelBindSpace.text = _currentPlayerMap.FindAction("Jump").bindings[0].ToDisplayString();
    }

    public void BeginRebinding(string _action) {
        _drop.interactable = false;
        InputSystem.onAnyButtonPress.CallOnce(button => {
            if (button is KeyControl key)
                RewriteBinding(key.name, _action);
            else
                BeginRebinding(_action);
        });
    }
    void RewriteBinding(string keyPath, string _action) {
        keyPath = "<Keyboard>/"+keyPath;
        var actionList = _action.Split('.');
        string inputAction = actionList[0];
        int bindingIndex = 0;
        if (actionList[0] != "Jump")
            bindingIndex = actionList[1] == "Up" || actionList[1] == "Right"? 2 : 1;
        
        string oldPath = _asset.FindActionMap(_currentPlayerMap.name).FindAction(inputAction).bindings[bindingIndex].path;

        for (int i = 0; i < _players.Count; i++) {
            _players[i].actions.
                FindActionMap(_currentPlayerMap.name).
                FindAction(inputAction).
                ChangeBindingWithPath(oldPath).
                WithPath(keyPath);
        }

        _asset.SaveBindingOverridesAsJson();
        _drop.interactable = true;
        RedrawUI();
    }
}
