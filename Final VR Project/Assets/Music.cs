using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] AudioSource here;
    [SerializeField] AudioClip [] clip;
    int i;
    int f;
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
        if (!here.isPlaying)
        {
            here.clip = clip[i];
            f = i;
            i = Random.Range(-1, 5);
            if(i == f)
            {
                i = Random.Range(-1, 5);
            }
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
