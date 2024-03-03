using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class MaintainPositionWithTrackedController : MonoBehaviour
{
    public GameObject xrRig;

    public bool leftHand;

    private InputDevice controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetupController();

        Vector3 position;
        if (controller.TryGetFeatureValue(CommonUsages.devicePosition, out position))
        {
            // NOTE: needs to be local position (i.e. position relative to rig / camera offset)
            transform.localPosition = position;
        }

        Quaternion orientation;
        if (controller.TryGetFeatureValue(CommonUsages.deviceRotation, out orientation))
        {
            // NOTE: needs to be local rotation (i.e. rotation relative to rig / camera offset)
            transform.localRotation = orientation;
        }

        bool triggerPressed;
        if (controller.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                var hitGO = hit.collider.gameObject;
                var button = hitGO.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                }
            }
        }
            
    }


    void SetupController()
    {
        var matchingControllerList = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;

        if (leftHand)
            desiredCharacteristics = desiredCharacteristics | InputDeviceCharacteristics.Left;
        else
            desiredCharacteristics = desiredCharacteristics | InputDeviceCharacteristics.Right;

        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, matchingControllerList);

        if (matchingControllerList.Count > 0)
            controller = matchingControllerList[0];
       // else
          //  Debug.Log("Unable to find controller for the " + (leftHand ? "left" : "right") + " hand");
    }
}
