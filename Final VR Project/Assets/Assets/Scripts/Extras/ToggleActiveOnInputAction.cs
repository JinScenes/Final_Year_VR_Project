using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleActiveOnInputAction : MonoBehaviour {

    public InputActionReference InputAction = default;
    public GameObject ToggleObject = default;

    private void OnEnable() {
        InputAction.action.performed += ToggleActive;
    }

    private void OnDisable() {
        InputAction.action.performed -= ToggleActive;
    }

    public void ToggleActive(InputAction.CallbackContext context) {
        if(ToggleObject) {
            ToggleObject.SetActive(!ToggleObject.activeSelf);
        }
    }
}

