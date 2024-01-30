using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSound : MonoBehaviour
{
    [SerializeField] AudioSource bounce_noise;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Surface")
        {
            bounce_noise.Play();
        }
    }
}
