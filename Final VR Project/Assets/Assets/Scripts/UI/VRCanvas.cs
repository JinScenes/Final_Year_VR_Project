using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(Canvas))]
public class VRCanvas : MonoBehaviour {

    void Start() {
        VRUISystem.Instance.AddCanvas(GetComponent<Canvas>());
    }
}

