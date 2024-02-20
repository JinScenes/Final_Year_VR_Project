using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSText : MonoBehaviour {
    Text text;
    float deltaTime = 0.0f;

    void Start() {
        text = GetComponent<Text>();
    }

    void Update() {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        text.text = Math.Ceiling(1.0f / deltaTime) + " FPS";
    }

    void OnGUI() {
        text.text = Math.Ceiling(1.0f / deltaTime) + " FPS";
    }
}