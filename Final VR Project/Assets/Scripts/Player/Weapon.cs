using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

namespace VR.Player.Weapon
{
    [RequireComponent(typeof(XRGrabInteractable))]
    [RequireComponent(typeof(Rigidbody))]
    public class Weapon : MonoBehaviour
    {
        [Header("Key Components"),
        Tooltip("The transform that allows object to face the correct way when grabbed")]
        public Transform grabTransform;
        
        [Tooltip("The object that will be instantiated to give the blood effects")]
        public GameObject bloodGameObject;

        [Header("Weapon Attributes"),
        Tooltip("The amount to take away the health of the enemy"),
        Range(0f, 100f)] public float damage;
    }
}