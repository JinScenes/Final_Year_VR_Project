using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    // Start is called before the first frame update
    public void VendWeapon(int index)
    {
        print("Dispensed Weapon: " + index);
    }
}
