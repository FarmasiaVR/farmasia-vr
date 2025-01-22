using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSparkleAudioFX : MonoBehaviour {

    private AudioSource audioFX;

    public void Start() {
        audioFX = GetComponent<AudioSource>();
    }

    public void PlayAudioFX() {
        audioFX.Play();
    }
}
