using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class MagCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pistol")
        {
            Debug.Log(GetComponent<BoxCollider>());

            GetComponent<XRGrabInteractable>().enabled = false; 
            GetComponent<Rigidbody>().freezeRotation = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }    
    
    private void OnTriggerExit(Collider other)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<BoxCollider>().enabled = true;
    }
}
