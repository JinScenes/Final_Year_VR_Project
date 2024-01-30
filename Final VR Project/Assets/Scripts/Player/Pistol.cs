using System.Collections;
using VR.Player.Weapon;
using UnityEngine;
using Enemy.AI;

public class Pistol : Weapon
{
    [Tooltip("The speeed which the magazine comes out of the gun"),
    Range(0f, 200f), SerializeField] private float ejectSpeed;

    [Tooltip("The time when the empty bullet is destroyed when instiated"),
    Range(0f, 200f), SerializeField] private float destroyTimer;        
        
    [Tooltip("Travel distance of the bullet"),
    Range(0f, 1000f), SerializeField] private float bulletSpeed;    
    
    [Tooltip("The distance that allows the pistol to deal damage"),
    Range(0f, 1000f), SerializeField] private float shootRange;

    ////////////////////////////////////////////////////////////////////////////////

    [Header("The Pistol Transform Positions")]
    [Tooltip("The muzzle position of the weapon"), 
    SerializeField] private Transform muzzlePosition;        
        
    [Tooltip("The ejection area of the weapon"), 
    SerializeField] private Transform ejectPosition;

    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    private LineRenderer line;
    private Rigidbody rb;
    private Animator anim;

    void Start()
    {
        //COMPONENTS
        anim = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();

        if(muzzlePosition == null)
        {
            muzzlePosition = transform;
        }

        if(anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        DebugRays();
    }

    public void Fire()
    {
        OnFire();
        StartCoroutine(Delay());
        anim.SetTrigger("Fire");
    }

    private void OnFire()
    {
        line.SetPosition(0, grabTransform.transform.position);
        line.SetPosition(1, grabTransform.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(muzzlePosition.transform.position, muzzlePosition.transform.forward, out hit, shootRange))
        {
            if (hit.transform.gameObject.CompareTag("Zombie"))
            {
                print("Enemey takes " + damage + " Damage");
                hit.transform.gameObject.GetComponent<ZombieAI>().EnemyDamage(damage);
                hit.transform.gameObject.SendMessage("EnemyDamage", damage);
                //Instantiate(bloodGameObject, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        if(muzzleFlashPrefab)
        {
            GameObject flash;
            flash = Instantiate(muzzleFlashPrefab, muzzlePosition.position, muzzlePosition.rotation);

            Destroy(flash, destroyTimer);
        }
    }

    private void EjectBullet()
    {
        if(!ejectPosition || !casingPrefab)
        {
            return;
        }

        GameObject eject;
        eject = Instantiate(casingPrefab, ejectPosition.position, ejectPosition.rotation) as GameObject;
        //eject.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectSpeed * 0.7f, ejectSpeed), 
        //    (ejectPosition.position - ejectPosition.right))
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        line.SetPosition(0, grabTransform.transform.position);
        line.SetPosition(1, grabTransform.transform.position);
    }

    private void DebugRays()
    {
        Debug.DrawRay(muzzlePosition.transform.position, muzzlePosition.transform.forward);
    }

    private void OnDrawGizmosSelected()
    {

    }
}