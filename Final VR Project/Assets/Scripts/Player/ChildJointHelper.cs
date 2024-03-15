using UnityEngine;

public class ChildJointHelper : MonoBehaviour
{
    [SerializeField] private GameObject targetChild;

    [SerializeField] private bool LockXPosition = false;
    [SerializeField] private bool LockYPosition = false;
    [SerializeField] private bool LockZPosition = false;

    [SerializeField] private bool LockXScale = true;
    [SerializeField] private bool LockYScale = true;
    [SerializeField] private bool LockZScale = true;

    [SerializeField] private bool LockXRotation = false;
    [SerializeField] private bool LockYRotation = false;
    [SerializeField] private bool LockZRotation = false;

    private Vector3 initialPosition;
    private Vector3 childInitiallRotation;
    private Vector3 initialScale;

    private Vector3 currentPosition;
    private Vector3 currentScale;
    private Vector3 currentRotation;

    private void Start()
    {
        initialPosition = transform.localPosition;
        childInitiallRotation = targetChild.transform.localEulerAngles;
        initialScale = transform.localScale;
    }

    private void lockPosition()
    {
        if (LockXPosition || LockYPosition || LockZPosition)
        {
            currentPosition = transform.localPosition;
            transform.localPosition = new Vector3
                (LockXPosition ? initialPosition.x : currentPosition.x, 
                LockYPosition ? initialPosition.y : currentPosition.y, 
                LockZPosition ? initialPosition.z : currentPosition.z);
        }

        if (LockXScale || LockYScale || LockZScale)
        {
            currentScale = transform.localScale;
            transform.localScale = new Vector3
                (LockXScale ? initialScale.x : currentScale.x, 
                LockYScale ? initialScale.y : currentScale.y,
                LockZScale ? initialScale.z : currentScale.z);
        }

        if (LockXRotation || LockYRotation || LockZRotation)
        {
            currentRotation = targetChild.transform.localEulerAngles;
            targetChild.transform.localEulerAngles = new Vector3
                (LockXRotation ? childInitiallRotation.x : currentRotation.x, 
                LockYRotation ? childInitiallRotation.y : currentRotation.y, 
                LockZRotation ? childInitiallRotation.z : currentRotation.z);
        }
    }

    private void LateUpdate()
    {
        lockPosition();
    }

    private void FixedUpdate()
    {
        lockPosition();
    }
}
