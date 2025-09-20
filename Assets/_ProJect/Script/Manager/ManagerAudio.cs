using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAudio : GenericSingleton<ManagerAudio>
{
    [SerializeField] private AudioSource soundFXObj;

    public void PlayFXSound(AudioClip audioClip, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObj);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(audioSource.gameObject,audioSource.clip.length);
    }

    public void PlayFXSound(AudioClip[] audioClip, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObj);
        audioSource.clip = audioClip[Random.Range(0,audioClip.Length)];
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}
