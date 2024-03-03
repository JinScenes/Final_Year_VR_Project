using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Ray : MonoBehaviour
{

    
    private Rigidbody rigidbod;

    bool mov = false;
    [SerializeField] float turnsped = 0.01f;

    
 

    Vector3 forward = new Vector3(0, 1, 0);
    [SerializeField] GameObject Hand;
    
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame

    void OnTriggerEnter(Collider other)
    {
        
            if (other.gameObject.CompareTag("Pup"))
            {
                rigidbod = other.GetComponent<Rigidbody>();
                rigidbod.useGravity = false;
                

                mov = true;


                rigidbod.drag = 1;
                Vector3 forward = new Vector3(1, 0, 0);
                float speed = 9.0f;
                other.transform.Translate(forward * speed * Time.deltaTime);


                if (mov == true)
                {

                    Vector3 targetdir = other.transform.position - Hand.transform.position;
                    Vector3 turn = Vector3.RotateTowards(other.transform.forward, targetdir, turnsped, 0.0f);
                    var turnquart = Quaternion.LookRotation(turn);
                    Quaternion rotatequar = Quaternion.Euler(other.transform.rotation.eulerAngles.x, turnquart.eulerAngles.y, other.transform.rotation.eulerAngles.z);
                    other.transform.rotation = rotatequar;

                }


                else
                {
                    mov = false;
                }

            }
            rigidbod.useGravity = true;
        
    }
}
