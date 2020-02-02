using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioScript : MonoBehaviour
{  
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip c)
    {
        source.PlayOneShot(c);
        Destroy(gameObject, c.length);
    }    
    
    public void Play(AudioClip c, float vol)
    {
        source.PlayOneShot(c, vol);
        Destroy(gameObject, c.length);
    }
}