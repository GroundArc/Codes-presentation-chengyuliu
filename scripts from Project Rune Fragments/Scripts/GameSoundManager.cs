using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{
    public static GameSoundManager Instance { get; private set; }

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip collectFragmentSound;
    [SerializeField] private AudioClip greedAbilitySound;
    [SerializeField] private AudioClip gluttonyAbilitySound;
    [SerializeField] private AudioClip envyAbilitySound;
    [SerializeField] private AudioClip slothAbilitySound;
    [SerializeField] private AudioClip wrathAbilitySound;
    [SerializeField] private AudioClip swithWeaponSound;
    [SerializeField] private AudioSource[] audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            audioSource[0].clip = backgroundMusic;
            audioSource[0].loop = true;
            audioSource[0].volume = 0.02f;
            audioSource[0].Play();
        }
    }


    public void PlayCollectFragmentSound()
    {
        if (collectFragmentSound != null)
        {
            audioSource[1].volume = 0.1f;
            audioSource[1].PlayOneShot(collectFragmentSound);
        }
    }

    public void PlayGreedAbilitySound()
    {
        if (greedAbilitySound != null)
        {
            audioSource[2].volume = 0.2f;
            audioSource[2].PlayOneShot(greedAbilitySound);
        }
    }

    public void PlayGluttonyAbilitySound()
    {
        if (gluttonyAbilitySound != null)
        {
            audioSource[3].PlayOneShot(gluttonyAbilitySound);
        }
    }

    public void PlayEnvyAbilitySound()
    {
        if (envyAbilitySound != null)
        {
            audioSource[4].PlayOneShot(envyAbilitySound);
        }
    }

    public void PlaySlothAbilitySound()
    {
        if (slothAbilitySound != null)
        {
            audioSource[5].volume = 0.2f;
            audioSource[5].PlayOneShot(slothAbilitySound);
        }
    }

    public void PlayWrathAbilitySound()
    {
        if (wrathAbilitySound != null)
        {
            audioSource[6].volume = 0.1f;
            audioSource[6].PlayOneShot(wrathAbilitySound);
        }
    }

    public void PlaySwitchWeaponSound()
    {
        if (swithWeaponSound != null)
        {
            audioSource[7].volume = 0.1f;
            audioSource[7].PlayOneShot(swithWeaponSound);
        }
    }

    public void PauseAllSounds()
    {
        for (int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].Pause();
        }
    }

    public void UnPauseAllSounds()
    {
        for (int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].UnPause();
        }
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].Stop();
        }
    }
}
