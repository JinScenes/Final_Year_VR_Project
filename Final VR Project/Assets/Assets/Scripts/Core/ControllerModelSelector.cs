using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerModelSelector : MonoBehaviour
{
    int disableIndex = 0;

    bool isQuitting = false;

    void OnEnable()
    {
        InputBridge.OnControllerFound += UpdateControllerModel;
    }

    public void UpdateControllerModel()
    {
        string controllerName = InputBridge.Instance.GetControllerName().ToLower();
        if (controllerName.Contains("quest 2"))
        {
            EnableChildController(0);
        }
        else if (controllerName.Contains("quest"))
        {
            EnableChildController(1);
        }
        else if (controllerName.Contains("rift"))
        {
            EnableChildController(2);
        }
    }

    public void EnableChildController(int childIndex)
    {
        transform.GetChild(disableIndex).gameObject.SetActive(false);
        transform.GetChild(childIndex).gameObject.SetActive(true);

        disableIndex = childIndex;
    }

    void OnDisable()
    {
        if (isQuitting)
        {
            return;
        }

        InputBridge.OnControllerFound -= UpdateControllerModel;
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }
}
