using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] AudioSource here;
    [SerializeField] AudioClip[] clip;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 1;
        here.clip = clip[0];
        here.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (here.isPlaying!)
        {
            if (i == 4)
            {
                i = 0;
            }
            here.clip = clip[i];
            i++;
            here.Play();
        }
    }
    void Stopm()
    {
        here.Pause();
    }
    void playm()
    {
        here.UnPause();
    }
}
