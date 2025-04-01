using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgAudioSource;
    public AudioSource systemAudioSource;
    public AudioSource playerAudioSource;
    public AudioSource EffectAudioSource;

    public AudioClip[] music;

    public AudioClip getItemSound;
    public AudioClip dieSound;
    public AudioClip playerScreem;

    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;
    public AudioClip doorLockedSound;
    public AudioClip doorBreakSound;
    public AudioClip drawerSound;
    public AudioClip mechanicalDoorLockedSound;
    public AudioClip mechanicalDoorOpenSound;
    public AudioClip doorHitSound;
    public AudioClip doorCrashSound;
    public AudioClip evCrashSound;
    public AudioClip lockerOpenSound;
    public AudioClip shutterOpenSound;
    public AudioClip printSound;

    public AudioClip keyPadButtonSound;
    public AudioClip keyPadSuccessSound;
    public AudioClip keyPadDenialSound;

    public AudioClip keyHoleSound;

    public AudioClip paperSound;

    public Slider systemVolume;
    public Slider bgmVolume;
    public Slider playerVolume;
    public Slider effectVolume;

    float systemvol;
    float bgmvol;
    float playervol;
    float effectvol;
    void Start()
    {
        systemvol = PlayerPrefs.GetFloat("systemvol", 1f);
        systemVolume.value = systemvol;
        systemAudioSource.volume = systemVolume.value;
        bgmvol = PlayerPrefs.GetFloat("bgmvol", 1f);
        bgmVolume.value = bgmvol;
        bgAudioSource.volume = bgmVolume.value;
        playervol = PlayerPrefs.GetFloat("playervol", 1f);
        playerVolume.value = playervol;
        playerAudioSource.volume = playerVolume.value;
        effectvol = PlayerPrefs.GetFloat("effectvol", 1f);
        effectVolume.value = effectvol;
        EffectAudioSource.volume = effectVolume.value;
    }
    void Update()
    {
        systemSoundSlider();
        bgmSoundSlider();
        playerSoundSlider();
        effectSoundSlider();
    }

    // 음악
    public void MusicPlay()
    {
        bgAudioSource.clip = music[0];
        bgAudioSource.loop = true;
        bgAudioSource.Play();
    }

    public void ChaseMusicPlay()
    {
        bgAudioSource.clip = music[1];
        bgAudioSource.loop = true;
        bgAudioSource.Play();
    }

    public void CatchSoundPlay()
    {
        bgAudioSource.clip = music[2];
        bgAudioSource.loop = false;
        bgAudioSource.Play();
    }

    public void StopMusicPlay()
    {
        bgAudioSource.Stop();
    }

    // 시스템 사운드
    public void GetItemSound()
    {
        systemAudioSource.clip = getItemSound;
        systemAudioSource.Play();
    }

    public void PaperSound()
    {
        systemAudioSource.clip = paperSound;
        systemAudioSource.Play();
    }

    public void DeadSound()
    {
        playerAudioSource.clip = playerScreem;
        playerAudioSource.Play();
        systemAudioSource.clip = dieSound;
        systemAudioSource.Play();
    }


    // 몹 사운드



    // 효과음
    public void DoorOpenSound()
    {
        EffectAudioSource.clip = doorOpenSound;
        EffectAudioSource.Play();
    }
    public void DoorCloseSound()
    {
        EffectAudioSource.clip = doorCloseSound;
        EffectAudioSource.Play();
    }
    public void DoorLockedSound()
    {
        EffectAudioSource.clip = doorLockedSound;
        EffectAudioSource.Play();
    }
    public void DoorBreakSound()
    {
        EffectAudioSource.clip = doorBreakSound;
        EffectAudioSource.Play();
    }
    public void DrawerSound()
    {
        EffectAudioSource.clip = drawerSound;
        EffectAudioSource.Play();
    }
    public void MechanicalDoorLockedSound()
    {
        EffectAudioSource.clip = mechanicalDoorLockedSound;
        EffectAudioSource.Play();
    }
    public void MechanicalDoorOpenSound()
    {
        EffectAudioSource.clip = mechanicalDoorOpenSound;
        EffectAudioSource.Play();
    }
    public void DoorHitSound() // 몬스터가 문 두드림
    {
        EffectAudioSource.clip = doorHitSound;
        EffectAudioSource.Play();
    }
    public void DoorCrashSound() // 몬스터가 문을 부숨
    {
        EffectAudioSource.clip = doorCrashSound;
        EffectAudioSource.Play();
    }
    public void EvCrashSound()
    {
        EffectAudioSource.clip = evCrashSound;
        EffectAudioSource.Play();
    }
    public void LockerOpenSound()
    {
        EffectAudioSource.clip = lockerOpenSound;
        EffectAudioSource.Play();
    }
    public void ShutterOpenSound()
    {
        EffectAudioSource.clip = shutterOpenSound;
        EffectAudioSource.Play();
    }

    public void KeyPadButton()
    {
        EffectAudioSource.clip = keyPadButtonSound;
        EffectAudioSource.Play();
    }
    public void KeyPadSuccess()
    {
        EffectAudioSource.clip = keyPadSuccessSound;
        EffectAudioSource.Play();
    }
    public void KeyPadDenial()
    {
        EffectAudioSource.clip = keyPadDenialSound;
        EffectAudioSource.Play();
    }

    public void KeyHoleSound()
    {
        EffectAudioSource.clip = keyHoleSound;
        EffectAudioSource.Play();
    }

    public void PrintSound()
    {
        EffectAudioSource.clip = printSound;
        EffectAudioSource.Play();
    }


    public void systemSoundSlider()
    {
        systemAudioSource.volume = systemVolume.value;
        systemvol = systemVolume.value;
        PlayerPrefs.SetFloat("systemvol", systemvol);
    }
    public void bgmSoundSlider()
    {
        bgAudioSource.volume = bgmVolume.value;
        bgmvol = bgmVolume.value;
        PlayerPrefs.SetFloat("bgmvol", bgmvol);
    }
    public void playerSoundSlider()
    {
        playerAudioSource.volume = playerVolume.value;
        playervol = playerVolume.value;
        PlayerPrefs.SetFloat("playervol", playervol);
    }
    public void effectSoundSlider()
    {
        EffectAudioSource.volume = effectVolume.value;
        effectvol = effectVolume.value;
        PlayerPrefs.SetFloat("effectvol", effectvol);
    }
}
