using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region Fields
    public static AudioManager Instance { get; private set; }
    private AudioClip clip;
    #endregion
    
    #region Public methods
    public void Play(string eventName) {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        switch (eventName) {
            case "lockedItem":
                clip = (AudioClip)Resources.Load("Item_locked");
                break;
            case "mistakeMessage":
                clip = (AudioClip)Resources.Load("Mistake_message");
                break;
            case "doneMessage":
                clip = (AudioClip)Resources.Load("Task_completed_beep1");
                break;
        }

        if (clip != null) {
            audio.PlayOneShot(clip, 1.0f);
        }
    }
    #endregion
}
