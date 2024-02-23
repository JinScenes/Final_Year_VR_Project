using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class BulletInsert : MonoBehaviour
{
    [SerializeField] private RaycastWeapon Weapon;
    [SerializeField] private AudioClip InsertSound;

    public string AcceptBulletName = "Bullet";

    void OnTriggerEnter(Collider other)
    {
        Grabbable grab = other.GetComponent<Grabbable>();
        if (grab != null)
        {
            if (grab.transform.name.Contains(AcceptBulletName))
            {

                if (Weapon.GetBulletCount() >= Weapon.MaxInternalAmmo)
                {
                    return;
                }

                grab.DropItem(false, true);
                grab.transform.parent = null;
                GameObject.Destroy(grab.gameObject);

                GameObject b = new GameObject();
                b.AddComponent<Bullet>();
                b.transform.parent = Weapon.transform;

                if (InsertSound)
                {
                    XRManager.Instance.PlaySpatialClipAt(InsertSound, transform.position, 1f, 0.5f);
                }
            }
        }
    }

}

