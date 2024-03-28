using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject lDoor, rDoor;
    void Start()
    {
       player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            lDoor.transform.Rotate(0, -90, 0);
            rDoor.transform.Rotate(0, 90, 0);
        }
    }
}