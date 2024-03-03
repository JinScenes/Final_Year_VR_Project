using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamageabl damage = collision.gameObject.GetComponent<IDamageabl>();
            damage.Dhealth(1);

        }
        else if(collision.gameObject.CompareTag("Head"))
        {
            IDamageabl damage = collision.gameObject.GetComponent<IDamageabl>();
            damage.Dhealth(1);

        }
    }

}