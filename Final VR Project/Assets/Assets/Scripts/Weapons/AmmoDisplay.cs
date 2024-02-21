using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private RaycastWeapon Weapon;
    [SerializeField] private Text AmmoLabel;

    private void OnGUI()
    {
        string loadedShot = Weapon.BulletInChamber ? "1" : "0";
        AmmoLabel.text = loadedShot + " / " + Weapon.GetBulletCount();
    }
}

