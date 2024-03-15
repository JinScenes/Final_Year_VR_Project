using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class AmmoDispenser : MonoBehaviour
{
    [Header("Grabbers")]
    [SerializeField] private Grabber LeftGrabber;
    [SerializeField] private Grabber RightGrabber;

    [Header("Clip Prefabs")]
    [SerializeField] private GameObject AmmoDispenserObject;
    [SerializeField] private GameObject PistolClip;
    [SerializeField] private GameObject ShotgunShell;
    [SerializeField] private GameObject SCARClip;
    [SerializeField] private GameObject M4A4Clip;
    [SerializeField] private GameObject AK74UClip;

    [Header("Clip Amounts")]
    public int CurrentPistolClips = 5;
    public int CurrentSCARClips = 5;
    public int CurrentM4A4Clips = 5;
    public int CurrentAK74UClip = 5;
    public int CurrentShotgunShells = 30;


    private void Update()
    {
        bool weaponEquipped = false;

        if (grabberHasWeapon(LeftGrabber) || grabberHasWeapon(RightGrabber))
        {
            weaponEquipped = true;
        }

        if (AmmoDispenserObject.activeSelf != weaponEquipped)
        {
            AmmoDispenserObject.SetActive(weaponEquipped);
        }
    }

    private bool grabberHasWeapon(Grabber grabbable)
    {

        if (grabbable == null || grabbable.HeldGrabbable == null)
        {
            return false;
        }

        string grabName = grabbable.HeldGrabbable.transform.name;

        if (grabName.Contains("Shotgun") || 
            grabName.Contains("M1911") || 
            grabName.Contains("SCAR") || 
            grabName.Contains("M4A4") ||
            grabName.Contains("AK-74U") ||
            grabName.Contains("Leader .50"))
        {
            return true;
        }

        return false;
    }

    public GameObject GetAmmo()
    {

        bool leftGrabberValid = LeftGrabber != null && LeftGrabber.HeldGrabbable != null;
        bool rightGrabberValid = RightGrabber != null && RightGrabber.HeldGrabbable != null;

        if (leftGrabberValid && LeftGrabber.HeldGrabbable.transform.name.Contains("Shotgun") && CurrentShotgunShells > 0)
        {
            CurrentShotgunShells--;
            return ShotgunShell;
        }
        else if (rightGrabberValid && RightGrabber.HeldGrabbable.transform.name.Contains("Shotgun") && CurrentShotgunShells > 0)
        {
            CurrentShotgunShells--;
            return ShotgunShell;
        }

        if (leftGrabberValid && LeftGrabber.HeldGrabbable.transform.name.Contains("SCAR") && CurrentSCARClips > 0)
        {
            CurrentSCARClips--;
            return SCARClip;
        }
        else if (rightGrabberValid && RightGrabber.HeldGrabbable.transform.name.Contains("SCAR") && CurrentSCARClips > 0)
        {
            CurrentSCARClips--;
            return SCARClip;
        }

        if (leftGrabberValid && LeftGrabber.HeldGrabbable.transform.name.Contains("M4A4") && CurrentM4A4Clips > 0)
        {
            CurrentM4A4Clips--;
            return M4A4Clip;
        }
        else if (rightGrabberValid && RightGrabber.HeldGrabbable.transform.name.Contains("M4A4") && CurrentM4A4Clips > 0)
        {
            CurrentM4A4Clips--;
            return M4A4Clip;
        }

        if (leftGrabberValid && LeftGrabber.HeldGrabbable.transform.name.Contains("M1911") && CurrentPistolClips > 0)
        {
            CurrentPistolClips--;
            return PistolClip;
        }
        else if (rightGrabberValid && RightGrabber.HeldGrabbable.transform.name.Contains("M1911") && CurrentPistolClips > 0)
        {
            CurrentPistolClips--;
            return PistolClip;
        }

        if (leftGrabberValid && LeftGrabber.HeldGrabbable.transform.name.Contains("AK-74U") && CurrentAK74UClip > 0)
        {
            CurrentAK74UClip--;
            return AK74UClip;
        }
        else if (rightGrabberValid && RightGrabber.HeldGrabbable.transform.name.Contains("AK-74U") && CurrentAK74UClip > 0)
        {
            CurrentAK74UClip--;
            return AK74UClip;
        }

        // REMINDER ADD SNIPER HERE
        return null;
    }

    public void GrabAmmo(Grabber grabber)
    {
        GameObject ammoClip = GetAmmo();
        if (ammoClip != null)
        {
            GameObject ammo = Instantiate(ammoClip, grabber.transform.position, grabber.transform.rotation) as GameObject;
            Grabbable grabbable = ammo.GetComponent<Grabbable>();

            GrabbableRingHelper grh = ammo.GetComponentInChildren<GrabbableRingHelper>();
            if (grh)
            {
                Destroy(grh);
                RingHelper r = ammo.GetComponentInChildren<RingHelper>();
                Destroy(r.gameObject);
            }

            ammo.transform.parent = grabber.transform;
            ammo.transform.localPosition = -grabbable.GrabPositionOffset;
            ammo.transform.parent = null;

            grabber.GrabGrabbable(grabbable);
        }
    }

    public virtual void AddAmmo(string AmmoName)
    {
        if (AmmoName.Contains("Shotgun"))
        {
            CurrentShotgunShells++;
        }
        else if (AmmoName.Contains("SCAR"))
        {
            CurrentSCARClips--;
        }
        else if (AmmoName.Contains("Pistol"))
        {
            CurrentPistolClips++;
        }
        else if (AmmoName.Contains("M4A4"))
        {
            CurrentM4A4Clips--;
        }
    }
}
