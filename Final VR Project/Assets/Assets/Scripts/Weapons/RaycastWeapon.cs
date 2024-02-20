using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum FiringType
{
    Semi,
    Automatic
}

public enum ReloadType
{
    InfiniteAmmo,
    ManualClip,
    InternalAmmo
}

public class RaycastWeapon : GrabbableEvents
{
    public float MaxRange = 25f;
    public float Damage = 25f;

    [Header("Select Fire")]
    public FiringType FiringMethod = FiringType.Semi;
    public ReloadType ReloadMethod = ReloadType.InfiniteAmmo;

    public float FiringRate = 0.2f;
    float lastShotTime;
    public float BulletImpactForce = 1000f;
    public float InternalAmmo = 0;
    public float MaxInternalAmmo = 10;
    public bool AutoChamberRounds = true;
    public bool MustChamberRounds = false;
    public bool AlwaysFireProjectile = false;
    public bool FireProjectileInSlowMo = true;

    public float SlowMoRateOfFire = 0.3f;
    public float ShotForce = 10f;
    public float BulletCasingForce = 3f;

    public bool LaserGuided = false;

    public Transform LaserPoint;
    public Vector3 RecoilForce = Vector3.zero;

    public float RecoilDuration = 0.3f;

    Rigidbody weaponRigid;

    public LayerMask ValidLayers;

    [SerializeField] private Transform MuzzlePointTransform;
    [SerializeField] private Transform EjectPointTransform;
    [SerializeField] private Transform TriggerTransform;
    [SerializeField] private Transform SlideTransform;

    [SerializeField] private Transform ChamberedBullet;

    [Header("Muzzle Flash")]
    [SerializeField] private GameObject MuzzleFlashObject;

    [Header("Prefabs")]
    [SerializeField] private GameObject BulletCasingPrefab;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject HitFXPrefab;

    [SerializeField] private AudioClip GunShotSound;

    [Range(0.0f, 1f)]
    public float GunShotVolume = 0.75f;
    public float slideSpeed = 1;

    public AudioClip EmptySound;

    [Range(0.0f, 1f)]
    public float EmptySoundVolume = 1f;
    public float SlideDistance = -0.028f;

    public bool ForceSlideBackOnLastShot = true;

    float minSlideDistance = 0.001f;

    public List<GrabbedControllerBinding> EjectInput = new List<GrabbedControllerBinding>() { GrabbedControllerBinding.Button2Down };
    public List<GrabbedControllerBinding> ReleaseSlideInput = new List<GrabbedControllerBinding>() { GrabbedControllerBinding.Button1Down };
    public List<GrabbedControllerBinding> ReloadInput = new List<GrabbedControllerBinding>() { GrabbedControllerBinding.Button2Down };

    public bool BulletInChamber = false;
    public bool EmptyBulletInChamber = false;

    [Header("Events")]
    [SerializeField] private UnityEvent onShootEvent;
    [SerializeField] private UnityEvent onAttachedAmmoEvent;
    [SerializeField] private UnityEvent onDetachedAmmoEvent;
    [SerializeField] private UnityEvent onWeaponChargedEvent;
    [SerializeField] private FloatEvent onDealtDamageEvent;
    [SerializeField] private RaycastHitEvent onRaycastHitEvent;

    protected WeaponSlide ws;

    protected bool slideForcedBack = false;
    protected bool readyToShoot = true;

    void Start()
    {
        weaponRigid = GetComponent<Rigidbody>();

        if (MuzzleFlashObject)
        {
            MuzzleFlashObject.SetActive(false);
        }

        ws = GetComponentInChildren<WeaponSlide>();

        updateChamberedBullet();
    }

    public override void OnTrigger(float triggerValue)
    {

        triggerValue = Mathf.Clamp01(triggerValue);

        if (TriggerTransform)
        {
            TriggerTransform.localEulerAngles = new Vector3(triggerValue * 15, 0, 0);
        }

        if (triggerValue <= 0.5)
        {
            readyToShoot = true;
            playedEmptySound = false;
        }

        if (readyToShoot && triggerValue >= 0.75f)
        {
            Shoot();

            readyToShoot = FiringMethod == FiringType.Automatic;
        }

        checkSlideInput();
        checkEjectInput();
        CheckReloadInput();

        updateChamberedBullet();

        base.OnTrigger(triggerValue);
    }

    void checkSlideInput()
    {
        for (int x = 0; x < ReleaseSlideInput.Count; x++)
        {
            if (InputBridge.Instance.GetGrabbedControllerBinding(ReleaseSlideInput[x], thisGrabber.HandSide))
            {
                UnlockSlide();
                break;
            }
        }
    }

    void checkEjectInput()
    {
        for (int x = 0; x < EjectInput.Count; x++)
        {
            if (InputBridge.Instance.GetGrabbedControllerBinding(EjectInput[x], thisGrabber.HandSide))
            {
                EjectMagazine();
                break;
            }
        }
    }

    public virtual void CheckReloadInput()
    {
        if (ReloadMethod == ReloadType.InternalAmmo)
        {
            for (int x = 0; x < ReloadInput.Count; x++)
            {
                if (InputBridge.Instance.GetGrabbedControllerBinding(EjectInput[x], thisGrabber.HandSide))
                {
                    Reload();
                    break;
                }
            }
        }
    }

    public virtual void UnlockSlide()
    {
        if (ws != null)
        {
            ws.UnlockBack();
        }
    }

    public virtual void EjectMagazine()
    {
        MagazineSlide ms = GetComponentInChildren<MagazineSlide>();
        if (ms != null)
        {
            ms.EjectMagazine();
        }
    }

    protected bool playedEmptySound = false;

    public virtual void Shoot()
    {

        float shotInterval = Time.timeScale < 1 ? SlowMoRateOfFire : FiringRate;
        if (Time.time - lastShotTime < shotInterval)
        {
            return;
        }

        if (!BulletInChamber && MustChamberRounds)
        {
            if (!playedEmptySound)
            {
                XRManager.Instance.PlaySpatialClipAt(EmptySound, transform.position, EmptySoundVolume, 0.5f);
                playedEmptySound = true;
            }

            return;
        }

        if (ws != null && ws.LockedBack)
        {
            XRManager.Instance.PlaySpatialClipAt(EmptySound, transform.position, EmptySoundVolume, 0.5f);
            return;
        }

        XRManager.Instance.PlaySpatialClipAt(GunShotSound, transform.position, GunShotVolume);

        if (thisGrabber != null)
        {
            input.VibrateController(0.1f, 0.2f, 0.1f, thisGrabber.HandSide);
        }

        bool useProjectile = AlwaysFireProjectile || (FireProjectileInSlowMo && Time.timeScale < 1);
        if (useProjectile)
        {
            GameObject projectile = Instantiate(ProjectilePrefab, MuzzlePointTransform.position, MuzzlePointTransform.rotation) as GameObject;
            Rigidbody projectileRigid = projectile.GetComponentInChildren<Rigidbody>();
            projectileRigid.AddForce(MuzzlePointTransform.forward * ShotForce, ForceMode.VelocityChange);

            Projectile proj = projectile.GetComponent<Projectile>();
            if (proj && !AlwaysFireProjectile)
            {
                proj.MarkAsRaycastBullet();
            }

            if (proj && LaserGuided)
            {
                if (LaserPoint == null)
                {
                    LaserPoint = MuzzlePointTransform;
                }

                proj.MarkAsLaserGuided(MuzzlePointTransform);
            }

            // Make sure we clean up this projectile
            Destroy(projectile, 20);
        }
        else
        {
            // Raycast to hit
            RaycastHit hit;
            if (Physics.Raycast(MuzzlePointTransform.position, MuzzlePointTransform.forward, out hit, MaxRange, ValidLayers, QueryTriggerInteraction.Ignore))
            {
                OnRaycastHit(hit);
            }
        }

        // Apply recoil
        ApplyRecoil();

        // We just fired this bullet
        BulletInChamber = false;

        // Try to load a new bullet into chamber         
        if (AutoChamberRounds)
        {
            chamberRound();
        }
        else
        {
            EmptyBulletInChamber = true;
        }

        // Unable to chamber bullet, force slide back
        if (!BulletInChamber)
        {
            // Do we need to force back the receiver?
            slideForcedBack = ForceSlideBackOnLastShot;

            if (slideForcedBack && ws != null)
            {
                ws.LockBack();
            }
        }

        // Call Shoot Event
        if (onShootEvent != null)
        {
            onShootEvent.Invoke();
        }

        // Store our last shot time to be used for rate of fire
        lastShotTime = Time.time;

        // Stop previous routine
        if (shotRoutine != null)
        {
            MuzzleFlashObject.SetActive(false);
            StopCoroutine(shotRoutine);
        }

        if (AutoChamberRounds)
        {
            shotRoutine = animateSlideAndEject();
            StartCoroutine(shotRoutine);
        }
        else
        {
            shotRoutine = doMuzzleFlash();
            StartCoroutine(shotRoutine);
        }
    }

    // Apply recoil by requesting sprinyness and apply a local force to the muzzle point
    public virtual void ApplyRecoil()
    {
        if (weaponRigid != null && RecoilForce != Vector3.zero)
        {

            // Make weapon springy for X seconds
            grab.RequestSpringTime(RecoilDuration);

            // Apply the Recoil Force
            weaponRigid.AddForceAtPosition(MuzzlePointTransform.TransformDirection(RecoilForce), MuzzlePointTransform.position, ForceMode.VelocityChange);
        }
    }

    // Hit something without Raycast. Apply damage, apply FX, etc.
    public virtual void OnRaycastHit(RaycastHit hit)
    {

        ApplyParticleFX(hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), hit.collider);

        // push object if rigidbody
        Rigidbody hitRigid = hit.collider.attachedRigidbody;
        if (hitRigid != null)
        {
            hitRigid.AddForceAtPosition(BulletImpactForce * MuzzlePointTransform.forward, hit.point);
        }

        // Damage if possible
        Damageable d = hit.collider.GetComponent<Damageable>();
        if (d)
        {
            d.DealDamage(Damage, hit.point, hit.normal, true, gameObject, hit.collider.gameObject);

            if (onDealtDamageEvent != null)
            {
                onDealtDamageEvent.Invoke(Damage);
            }
        }

        // Call event
        if (onRaycastHitEvent != null)
        {
            onRaycastHitEvent.Invoke(hit);
        }
    }

    public virtual void ApplyParticleFX(Vector3 position, Quaternion rotation, Collider attachTo)
    {
        if (HitFXPrefab)
        {
            GameObject impact = Instantiate(HitFXPrefab, position, rotation) as GameObject;

            // Attach bullet hole to object if possible
            BulletHole hole = impact.GetComponent<BulletHole>();
            if (hole)
            {
                hole.TryAttachTo(attachTo);
            }
        }
    }

    /// <summary>
    /// Something attached ammo to us
    /// </summary>
    public virtual void OnAttachedAmmo()
    {

        // May have ammo loaded
        updateChamberedBullet();

        if (onAttachedAmmoEvent != null)
        {
            onAttachedAmmoEvent.Invoke();
        }
    }

    // Ammo was detached from the weapon
    public virtual void OnDetachedAmmo()
    {
        // May have ammo loaded / unloaded
        updateChamberedBullet();

        if (onDetachedAmmoEvent != null)
        {
            onDetachedAmmoEvent.Invoke();
        }
    }

    public virtual int GetBulletCount()
    {
        if (ReloadMethod == ReloadType.InfiniteAmmo)
        {
            return 9999;
        }
        else if (ReloadMethod == ReloadType.InternalAmmo)
        {
            return (int)InternalAmmo;
        }
        else if (ReloadMethod == ReloadType.ManualClip)
        {
            return GetComponentsInChildren<Bullet>(false).Length;
        }

        // Default to bullet count
        return GetComponentsInChildren<Bullet>(false).Length;
    }

    public virtual void RemoveBullet()
    {

        // Don't remove bullet here
        if (ReloadMethod == ReloadType.InfiniteAmmo)
        {
            return;
        }

        else if (ReloadMethod == ReloadType.InternalAmmo)
        {
            InternalAmmo--;
        }
        else if (ReloadMethod == ReloadType.ManualClip)
        {
            Bullet firstB = GetComponentInChildren<Bullet>(false);
            // Deactivate gameobject as this bullet has been consumed
            if (firstB != null)
            {
                Destroy(firstB.gameObject);
            }
        }

        // Whenever we remove a bullet is a good time to check the chamber
        updateChamberedBullet();
    }


    public virtual void Reload()
    {
        InternalAmmo = MaxInternalAmmo;
    }

    void updateChamberedBullet()
    {
        if (ChamberedBullet != null)
        {
            ChamberedBullet.gameObject.SetActive(BulletInChamber || EmptyBulletInChamber);
        }
    }

    void chamberRound()
    {

        int currentBulletCount = GetBulletCount();

        if (currentBulletCount > 0)
        {
            // Remove the first bullet we find in the clip                
            RemoveBullet();

            // That bullet is now in chamber
            BulletInChamber = true;
        }
        // Unable to chamber a bullet
        else
        {
            BulletInChamber = false;
        }
    }

    protected IEnumerator shotRoutine;

    // Randomly scale / rotate to make them seem different
    void randomizeMuzzleFlashScaleRotation()
    {
        MuzzleFlashObject.transform.localScale = Vector3.one * Random.Range(0.75f, 1.5f);
        MuzzleFlashObject.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 90f));
    }

    public virtual void OnWeaponCharged(bool allowCasingEject)
    {

        // Already bullet in chamber, eject it
        if (BulletInChamber && allowCasingEject)
        {
            ejectCasing();
        }
        else if (EmptyBulletInChamber && allowCasingEject)
        {
            ejectCasing();
            EmptyBulletInChamber = false;
        }

        chamberRound();

        // Slide is no longer forced back if weapon was just charged
        slideForcedBack = false;

        if (onWeaponChargedEvent != null)
        {
            onWeaponChargedEvent.Invoke();
        }
    }

    protected virtual void ejectCasing()
    {
        GameObject shell = Instantiate(BulletCasingPrefab, EjectPointTransform.position, EjectPointTransform.rotation) as GameObject;
        Rigidbody rb = shell.GetComponentInChildren<Rigidbody>();

        if (rb)
        {
            rb.AddRelativeForce(Vector3.right * BulletCasingForce, ForceMode.VelocityChange);
        }

        // Clean up shells
        GameObject.Destroy(shell, 5);
    }

    protected virtual IEnumerator doMuzzleFlash()
    {
        MuzzleFlashObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);

        randomizeMuzzleFlashScaleRotation();
        yield return new WaitForSeconds(0.05f);

        MuzzleFlashObject.SetActive(false);
    }

    // Animate the slide back, eject casing, pull slide back
    protected virtual IEnumerator animateSlideAndEject()
    {

        // Start Muzzle Flash
        MuzzleFlashObject.SetActive(true);

        int frames = 0;
        bool slideEndReached = false;
        Vector3 slideDestination = new Vector3(0, 0, SlideDistance);

        if (SlideTransform)
        {
            while (!slideEndReached)
            {


                SlideTransform.localPosition = Vector3.MoveTowards(SlideTransform.localPosition, slideDestination, Time.deltaTime * slideSpeed);
                float distance = Vector3.Distance(SlideTransform.localPosition, slideDestination);

                if (distance <= minSlideDistance)
                {
                    slideEndReached = true;
                }

                frames++;

                // Go ahead and update muzzleflash in sync with slide
                if (frames < 2)
                {
                    randomizeMuzzleFlashScaleRotation();
                }
                else
                {
                    slideEndReached = true;
                    MuzzleFlashObject.SetActive(false);
                }

                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            yield return new WaitForEndOfFrame();
            randomizeMuzzleFlashScaleRotation();
            yield return new WaitForEndOfFrame();

            MuzzleFlashObject.SetActive(false);
            slideEndReached = true;
        }

        // Set Slide Position
        if (SlideTransform)
        {
            SlideTransform.localPosition = slideDestination;
        }

        yield return new WaitForEndOfFrame();
        MuzzleFlashObject.SetActive(false);

        // Eject Shell
        ejectCasing();

        // Pause for shell to eject before returning slide
        yield return new WaitForEndOfFrame();


        if (!slideForcedBack && SlideTransform != null)
        {
            // Slide back to original position
            frames = 0;
            bool slideBeginningReached = false;
            while (!slideBeginningReached)
            {

                SlideTransform.localPosition = Vector3.MoveTowards(SlideTransform.localPosition, Vector3.zero, Time.deltaTime * slideSpeed);
                float distance = Vector3.Distance(SlideTransform.localPosition, Vector3.zero);

                if (distance <= minSlideDistance)
                {
                    slideBeginningReached = true;
                }

                if (frames > 2)
                {
                    slideBeginningReached = true;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}