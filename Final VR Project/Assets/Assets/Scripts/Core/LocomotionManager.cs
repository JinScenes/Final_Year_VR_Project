using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class LocomotionManager : MonoBehaviour
{
    [Header("Locomotion Type")]
    [SerializeField] private LocomotionType DefaultLocomotion = LocomotionType.Teleport;
    [SerializeField] private List<ControllerBinding> locomotionToggleInput = new List<ControllerBinding>() { ControllerBinding.None };
    [SerializeField] private InputActionReference LocomotionToggleAction;

    [SerializeField] private LocomotionType SelectedLocomotion { get { return selectedLocomotion; } }
    private LocomotionType selectedLocomotion = LocomotionType.Teleport;

    private bool LoadLocomotionFromPrefs = false;

    private PlayerController player;
    private PlayerTeleport teleport;
    private SmoothLocomotion smoothLocomotion;

    private void Start()
    {
        player = GetComponentInChildren<PlayerController>();
        teleport = GetComponentInChildren<PlayerTeleport>();

        if (LoadLocomotionFromPrefs)
        {
            ChangeLocomotion(PlayerPrefs.GetInt("LocomotionSelection", 0) == 0 ? LocomotionType.Teleport : LocomotionType.SmoothLocomotion, false);
        }
        else
        {
            ChangeLocomotion(DefaultLocomotion, false);
        }
    }

    bool actionToggle = false;

    private void Update()
    {
        if (!actionToggle)
        {
            CheckControllerToggleInput();
        }

        actionToggle = false;
    }

    public virtual void CheckControllerToggleInput()
    {
        for (int x = 0; x < locomotionToggleInput.Count; x++)
        {
            if (XRInput.Instance.GetControllerBindingValue(locomotionToggleInput[x]))
            {
                LocomotionToggle();
            }
        }
    }

    private void OnEnable()
    {
        if (LocomotionToggleAction)
        {
            LocomotionToggleAction.action.Enable();
            LocomotionToggleAction.action.performed += OnLocomotionToggle;
        }
    }

    private void OnDisable()
    {
        if (LocomotionToggleAction)
        {
            LocomotionToggleAction.action.Disable();
            LocomotionToggleAction.action.performed -= OnLocomotionToggle;
        }
    }

    public void OnLocomotionToggle(InputAction.CallbackContext context)
    {
        actionToggle = true;
        LocomotionToggle();
    }

    public void LocomotionToggle()
    {
        ChangeLocomotion(SelectedLocomotion == LocomotionType.SmoothLocomotion ? LocomotionType.Teleport : LocomotionType.SmoothLocomotion, LoadLocomotionFromPrefs);
    }

    public void UpdateTeleportStatus()
    {
        teleport.enabled = SelectedLocomotion == LocomotionType.Teleport;
    }

    public void ChangeLocomotion(LocomotionType locomotionType, bool save)
    {
        ChangeLocomotionType(locomotionType);

        if (save)
        {
            PlayerPrefs.SetInt("LocomotionSelection", locomotionType == LocomotionType.Teleport ? 0 : 1);
        }

        UpdateTeleportStatus();
    }

    public void ChangeLocomotionType(LocomotionType loc)
    {

        selectedLocomotion = loc;

        if (smoothLocomotion == null)
        {
            smoothLocomotion = GetComponentInChildren<SmoothLocomotion>();
        }

        if (teleport == null)
        {
            teleport = GetComponentInChildren<PlayerTeleport>();
        }

        toggleTeleport(selectedLocomotion == LocomotionType.Teleport);
        toggleSmoothLocomotion(selectedLocomotion == LocomotionType.SmoothLocomotion);
    }

    private void toggleTeleport(bool enabled)
    {
        if (enabled)
        {
            teleport.EnableTeleportation();
        }
        else
        {
            teleport.DisableTeleportation();
        }
    }

    private void toggleSmoothLocomotion(bool enabled)
    {
        if (smoothLocomotion)
        {
            smoothLocomotion.enabled = enabled;
        }
    }

    public void ToggleLocomotionType()
    {
        // Toggle based on last value
        if (selectedLocomotion == LocomotionType.SmoothLocomotion)
        {
            ChangeLocomotionType(LocomotionType.Teleport);
        }
        else
        {
            ChangeLocomotionType(LocomotionType.SmoothLocomotion);
        }
    }
}