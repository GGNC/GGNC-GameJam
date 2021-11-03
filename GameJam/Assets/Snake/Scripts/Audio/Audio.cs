using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    [SerializeField]
    private AudioSource Player;
    [SerializeField]
    private AudioClip Sound;
    public void Play()
    {
        Player.PlayOneShot(Sound); 
    }
}
