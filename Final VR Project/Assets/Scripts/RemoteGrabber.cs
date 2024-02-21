using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum RemoteGrabType
{
    Trigger,
    Raycast,
    Spherecast
}

public class RemoteGrabber : MonoBehaviour
{

    [SerializeField] private RemoteGrabType PhysicsCheckType = RemoteGrabType.Trigger;

    [Tooltip("If PhysicsCheckType = Trigger and this is true, an additonal raycast check will occur to check for obstacles in the way")]
    [SerializeField] private bool TriggerRequiresRaycast = true;

    [SerializeField] private float RaycastLength = 20f;

    [SerializeField] private float SphereCastLength = 20f;
    [SerializeField] private float SphereCastRadius = 0.05f;

    [SerializeField] private LayerMask RemoteGrabLayers = ~0;

    [SerializeField] private GrabbablesInTrigger ParentGrabber;

    [SerializeField] private Collider _lastColliderHit = null;

    private void Start()
    {
        if (PhysicsCheckType == RemoteGrabType.Trigger && GetComponent<Collider>() == null)
        {
            Debug.LogWarning("Remote Grabber set to 'Trigger', but no Trigger Collider was found. You may need to add a collider, or switch to a different physics check type.");
        }

        if (PhysicsCheckType == RemoteGrabType.Trigger && TriggerRequiresRaycast && ParentGrabber != null)
        {
            ParentGrabber.RaycastRemoteGrabbables = true;
        }
    }

    public virtual void Update()
    {
        if (PhysicsCheckType == RemoteGrabType.Raycast)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, RaycastLength, RemoteGrabLayers))
            {
                ObjectHit(hit.collider);
            }
            else if (_lastColliderHit != null)
            {
                RemovePreviousHitObject();
            }
        }
        else if (PhysicsCheckType == RemoteGrabType.Spherecast)
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, SphereCastRadius, transform.forward, out hit, SphereCastLength))
            {
                ObjectHit(hit.collider);
            }
            else if (_lastColliderHit != null)
            {
                RemovePreviousHitObject();
            }
        }
    }

    private void ObjectHit(Collider colliderHit)
    {
        if (ParentGrabber == null)
        {
            return;
        }

        if (_lastColliderHit != colliderHit)
        {
            RemovePreviousHitObject();
        }

        _lastColliderHit = colliderHit;

        if (_lastColliderHit.gameObject.TryGetComponent(out Grabbable grabObject))
        {
            ParentGrabber.AddValidRemoteGrabbable(_lastColliderHit, grabObject);
            return;
        }

        if (_lastColliderHit.gameObject.TryGetComponent(out GrabbableChild gc))
        {
            ParentGrabber.AddValidRemoteGrabbable(_lastColliderHit, gc.ParentGrabbable);
            return;
        }
    }

    public void RemovePreviousHitObject()
    {
        if (_lastColliderHit == null) return;

        if (_lastColliderHit.TryGetComponent(out Grabbable grabObject))
        {
            ParentGrabber.RemoveValidRemoteGrabbable(_lastColliderHit, grabObject);
            return;
        }

        if (_lastColliderHit.TryGetComponent(out GrabbableChild gc))
        {
            ParentGrabber.RemoveValidRemoteGrabbable(_lastColliderHit, gc.ParentGrabbable);
            return;
        }

        _lastColliderHit = null;
    }

    Grabbable grabObject;
    GrabbableChild gc;

    void OnTriggerEnter(Collider other)
    {
        if (ParentGrabber == null || PhysicsCheckType != RemoteGrabType.Trigger)
        {
            return;
        }

        if (other.gameObject.layer == 2)
        {
            return;
        }

        if (other.gameObject.isStatic)
        {
            return;
        }

        grabObject = other.GetComponent<Grabbable>();
        if (grabObject != null && ParentGrabber != null)
        {
            ParentGrabber.AddValidRemoteGrabbable(other, grabObject);
            return;
        }

        gc = other.GetComponent<GrabbableChild>();
        if (gc != null && ParentGrabber != null)
        {
            ParentGrabber.AddValidRemoteGrabbable(other, gc.ParentGrabbable);
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (ParentGrabber == null || PhysicsCheckType != RemoteGrabType.Trigger)
        {
            return;
        }

        grabObject = other.GetComponent<Grabbable>();
        if (grabObject != null && ParentGrabber != null)
        {
            ParentGrabber.RemoveValidRemoteGrabbable(other, grabObject);
            return;
        }

        gc = other.GetComponent<GrabbableChild>();
        if (gc != null && ParentGrabber != null)
        {
            ParentGrabber.RemoveValidRemoteGrabbable(other, gc.ParentGrabbable);
            return;
        }
    }

    public bool ShowGizmos = true;

    void OnDrawGizmos()
    {

        if (!this.isActiveAndEnabled)
        {
            return;
        }

        if (ShowGizmos)
        {
            if (PhysicsCheckType == RemoteGrabType.Raycast)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * RaycastLength, Color.green);
            }
            else if (PhysicsCheckType == RemoteGrabType.Spherecast)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * SphereCastLength, Color.green);

                DrawWireCapsule(transform.position + (transform.forward * SphereCastLength / 2), transform.rotation * Quaternion.Euler(90f, 0, 0), SphereCastRadius, SphereCastLength, Color.green);
            }
        }
    }

    public void DrawWireCapsule(Vector3 position, Quaternion rotation, float radius, float height, Color color)
    {

        UnityEditor.Handles.color = color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(position, rotation, UnityEditor.Handles.matrix.lossyScale);

        using (new UnityEditor.Handles.DrawingScope(angleMatrix))
        {
            var pointOffset = (height - (radius * 2)) / 2;

            // Draw sideways
            UnityEditor.Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
            UnityEditor.Handles.DrawLine(new Vector3(0, pointOffset, -radius), new Vector3(0, -pointOffset, -radius));
            UnityEditor.Handles.DrawLine(new Vector3(0, pointOffset, radius), new Vector3(0, -pointOffset, radius));
            UnityEditor.Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);

            // Draw frontways
            UnityEditor.Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
            UnityEditor.Handles.DrawLine(new Vector3(-radius, pointOffset, 0), new Vector3(-radius, -pointOffset, 0));
            UnityEditor.Handles.DrawLine(new Vector3(radius, pointOffset, 0), new Vector3(radius, -pointOffset, 0));
            UnityEditor.Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);

            // Draw center
            UnityEditor.Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, radius);
            UnityEditor.Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, radius);
        }
    }
}