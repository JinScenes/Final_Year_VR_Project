using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasGroup : MonoBehaviour {

    public List<GameObject> CanvasObjects;
        
    public void ActivateCanvas(int CanvasIndex) {
        for(int x = 0; x < CanvasObjects.Count; x++) {
            if(CanvasObjects[x] != null) {
                CanvasObjects[x].SetActive(x == CanvasIndex);
            }
        }
    }
}

