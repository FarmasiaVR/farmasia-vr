using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipType {
    LockedItem,
    MistakeMessage,
    TaskCompletedBeep,
    Diu
}

public class AudioManager {
    #region Fields
    private static GameObject defaultSource;
    private static string audioFileLocation = "Audio/Clips/";
    #endregion
    
    static AudioManager() {
        defaultSource = GameObject.FindWithTag("DefaultAudioSource");
        if (defaultSource == null) {
            Logger.Error("Did not find a gameObject tagged with DefaultAudioSource, playing sounds will not work if source is not given as a parameter");
        }
    }
    
    #region Public methods
    public static void Play(AudioClipType type, GameObject source = null, bool enableSpatialSound = false) {

        if (source == null) {
            source = defaultSource;
            if (source == null) {
                Logger.Error("Missing gameObject tagged with DefaultAudioSource");
                return;
            }
        }

        AudioClip clip = null;
        AudioSource audio = source.GetComponent<AudioSource>();
        if (audio == null) audio = source.AddComponent<AudioSource>();

        if (enableSpatialSound) {
            audio.spatialBlend = 1;
        } else {
            audio.spatialBlend = 0;
        }

        switch (type) {
            case AudioClipType.LockedItem:
                clip = Resources.Load<AudioClip>(audioFileLocation + "Item_locked");
                break;
            case AudioClipType.MistakeMessage:
                clip = Resources.Load<AudioClip>(audioFileLocation + "Mistake_message");
                break;
            case AudioClipType.TaskCompletedBeep:
                clip = Resources.Load<AudioClip>(audioFileLocation + "Task_completed_beep1");
                break;
            case AudioClipType.Diu:
                clip = Resources.Load<AudioClip>(audioFileLocation + "Minus_point");
                break;
        }

        if (clip != null) {
            audio.PlayOneShot(clip, 1.0f);
        } else {
            Logger.Error("Did not find sound clip");
        }
    }
    #endregion
}
