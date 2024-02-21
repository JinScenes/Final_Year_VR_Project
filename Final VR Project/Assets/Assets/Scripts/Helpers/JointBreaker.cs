using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class JointBreaker : MonoBehaviour {

    public float BreakDistance = 0.25f;
    public float JointDistance;

    public bool DestroyJointOnBreak = true;

    public GrabberEvent OnBreakEvent;

    Joint theJoint;

    Vector3 startPos;

    bool brokeJoint = false;

    void Start() 
    {
        startPos = transform.localPosition;
        theJoint = GetComponent<Joint>();
    }

    void Update() 
    {
        JointDistance = Vector3.Distance(transform.localPosition, startPos);

        if(!brokeJoint && JointDistance > BreakDistance) {
            BreakJoint();
        }
    }

    public void BreakJoint() {

        if(DestroyJointOnBreak &&  theJoint) {
            Destroy(theJoint);
        }

        if (OnBreakEvent != null) {

            var heldGrabbable = GetComponent<Grabbable>();
            if(heldGrabbable && heldGrabbable.GetPrimaryGrabber() ) {
                brokeJoint = true;
                OnBreakEvent.Invoke(heldGrabbable.GetPrimaryGrabber());
            }
        }
    }
}

