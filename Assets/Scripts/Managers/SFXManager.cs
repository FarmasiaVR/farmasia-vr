using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SFXManager : MonoBehaviour
{
    private AudioSource audioSource;
    private GameObject playerCamera;
    public AudioClip successSound;
    public AudioClip mistakeSound;

    public void Awake()
    {
        audioSource = new GameObject("SFXPlayer").AddComponent<AudioSource>();
        audioSource.maxDistance = 100000;
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void SetAudioPosition()
    {
        audioSource.transform.position = playerCamera.transform.position;
    }

    public void PlaySuccessSound()
    {
        audioSource.PlayOneShot(successSound);
    }

    public void PlayMistakeSound()
    {
        audioSource.PlayOneShot(mistakeSound);
    }
}
