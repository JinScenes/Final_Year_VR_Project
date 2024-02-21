using UnityEngine;

public class ControllerModelSelector : MonoBehaviour
{
    private bool isQuitting = false;
    private int disableIndex = 0;

    private void OnEnable()
    {
        InputBridge.OnControllerFound += UpdateControllerModel;
    }

    public void UpdateControllerModel()
    {
        string controllerName = InputBridge.Instance.GetControllerName().ToLower();

        if (controllerName.Contains("quest 2") || controllerName.Contains("quest 3"))
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

    private void OnDisable()
    {
        if (isQuitting)
        {
            return;
        }

        InputBridge.OnControllerFound -= UpdateControllerModel;
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
}
