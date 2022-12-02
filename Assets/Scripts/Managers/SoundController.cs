using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
     public static SoundController Instance;


     public AudioClip bubblePop;
     

     private AudioSource _audioSource;
     
     private void Awake()
     {
          Instance = this;
          _audioSource = GetComponent<AudioSource>();
     }


     public void PlayBubblePop()
     {
          _audioSource.PlayOneShot(bubblePop);
     }
     
}
