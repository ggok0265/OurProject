using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSound : MonoBehaviour
{
    public GameObject foot;
    public AudioSource footAudioSource;
    public AudioSource audioSource;
    public GameObject playerNeck;
    public AudioSource pNAudioSource;

    public AudioClip breathe;
    public AudioClip roar;
    public AudioClip suspect;
    public AudioClip screem;

    public AudioClip[] isRun;

    public AudioClip walk;
    public AudioClip run;

    public AudioClip beforeDead;
    public AudioClip dieSound;

    public void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        footAudioSource = foot.GetComponent<AudioSource>();
        pNAudioSource = playerNeck.GetComponent<AudioSource>();
    }

    public void BreatheSound()
    {
        audioSource.clip = breathe;
        audioSource.Play();
    }
    public void RoarSound()
    {
        audioSource.clip = roar;
        audioSource.Play();
    }

    public void SuspectSound()
    {
        audioSource.clip = suspect;
        audioSource.Play();
    }

    public void ScreemSound()
    {
        audioSource.clip = screem;
        audioSource.Play();
    }

    // 플레이어 사운드
    public void CatchedSound()
    {        
        pNAudioSource.clip = beforeDead;
        pNAudioSource.Play();
    }

    public void Die() // UI등도 여기서 제어
    {
        gameObject.GetComponent<MonsterAI>().Managers.GetComponent<UIManager>().bloodEffect.SetActive(true);
        pNAudioSource.clip = dieSound;
        pNAudioSource.Play();
    }

    public void IsRun()
    {
        int num = Random.Range(0, 2);
        int num2 = Random.Range(0, 2);
        if(num2 > 0)
        {
            audioSource.clip = isRun[num];
            audioSource.Play();
        }
    }

    public void WalkSound1()
    {
        footAudioSource.clip = walk;
        footAudioSource.Play();
    }
    public void RunSound1()
    {
        footAudioSource.clip = run;
        footAudioSource.Play();
    }
}