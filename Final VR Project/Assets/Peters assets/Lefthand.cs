using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Lefthand : MonoBehaviour
{
    private InputDevice controller;
    [SerializeField] GameObject Ray;

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

            Ray.SetActive(true);
        }
        else
        {
            Ray.SetActive(false);
        }
    }



    void SetupController()
    {
        var matchingControllerList = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;
        desiredCharacteristics = desiredCharacteristics | InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, matchingControllerList);

        if (matchingControllerList.Count > 0)
        {
            controller = matchingControllerList[0];
        }
    }
}