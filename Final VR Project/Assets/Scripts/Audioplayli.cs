using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audioplayli : MonoBehaviour
{
    public AudioClip[] sountr;

    private AudioSource Au;
    int i;
    // Start is called before the first frame update
    void Awake()
    {
        i = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Au.isPlaying)
        {
            Au.clip = sountr[i];
            Au.Play();
            i++;
            if (i>= sountr.Length)
            {
                i = 0;
            }
        }
    }
}
