using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobFootSound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip walk;
    public AudioClip run;

    public void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    //public void WalkSound()
    //{
    //    Debug.Log("w");
    //    audioSource.clip = walk;
    //    audioSource.loop = false;
    //    audioSource.Play();
    //}
    //public void RunSound()
    //{
    //    Debug.Log("R");
    //    audioSource.clip = run;
    //    audioSource.loop = false;
    //    audioSource.Play();
    //}
}
