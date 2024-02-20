using UnityEngine;

public class ArmSwingLocomotion : MonoBehaviour
{

    public CharacterController characterController;
    public Transform ForwardDirection;

    public float SpeedModifier = 5f;

    public float MinInput = 0.1f;

    public bool MustBeHoldingLeftTrigger = true;
    public bool MustBeHoldingRightTrigger = true;

    public bool MustBeHoldingLeftGrip = false;
    public bool MustBeHoldingRightGrip = false;

    public float VelocitySum
    {
        get
        {
            return leftVelocity + rightVelocity;
        }
    }
    float leftVelocity;
    float rightVelocity;

    void Start()
    {
        if (characterController == null)
        {
            characterController = GetComponentInChildren<CharacterController>();
        }

        if (ForwardDirection == null)
        {
            ForwardDirection = transform;
        }
    }

    void Update()
    {
        UpdateVelocities();
        UpdateMovement();
    }

    public virtual void UpdateMovement()
    {
        if (characterController && VelocitySum > MinInput)
        {
            characterController.Move(ForwardDirection.forward * VelocitySum * SpeedModifier * Time.deltaTime);
        }
    }

    public void UpdateVelocities()
    {
        if (LeftInputReady())
        {
            leftVelocity = InputBridge.Instance.GetControllerVelocity(ControllerHand.Left).magnitude;
        }
        else
        {
            leftVelocity = 0;
        }

        if (RightInputReady())
        {
            rightVelocity = InputBridge.Instance.GetControllerVelocity(ControllerHand.Right).magnitude;
        }
        else
        {
            rightVelocity = 0;
        }
    }

    public virtual bool LeftInputReady()
    {

        if (MustBeHoldingLeftGrip && InputBridge.Instance.LeftGrip < 0.1f)
        {
            return false;
        }

        if (MustBeHoldingLeftTrigger && InputBridge.Instance.LeftTrigger < 0.1f)
        {
            return false;
        }

        return true;
    }

    public virtual bool RightInputReady()
    {

        if (MustBeHoldingRightGrip && InputBridge.Instance.RightGrip < 0.1f)
        {
            return false;
        }

        if (MustBeHoldingRightTrigger && InputBridge.Instance.RightTrigger < 0.1f)
        {
            return false;
        }

        return true;
    }
}