using System;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipType {
    LockedItem,
    MistakeMessage,
    TaskCompletedBeep,
    MinusPoint,
}

public class AudioManager {

    private const string AUDIO_FILE_LOCATION = "Audio/Clips/";

    private Dictionary<AudioClipType, AudioClipData> audioClips;

    public AudioManager() {
        audioClips = new Dictionary<AudioClipType, AudioClipData>();
        audioClips.Add(AudioClipType.LockedItem, new AudioClipData("LockedItemAudioSource", "Item_locked", 1));
        audioClips.Add(AudioClipType.MistakeMessage, new AudioClipData("MistakeAudioSource", "Mistake_message", 1));
        audioClips.Add(AudioClipType.TaskCompletedBeep, new AudioClipData("TaskCompletedAudioSource", "Task_completed_beep1", 1));
        audioClips.Add(AudioClipType.MinusPoint, new AudioClipData("MinusPointAudioSource", "Minus_point", 1));
    }

    public void Play(AudioClipType type, GameObject audioSourceObject = null, float spatialBlend = 0) {
        try {
            AudioSource audioSrc = GetAudioSource(audioSourceObject, type);
            if (audioSrc == null) {
                Logger.Warning("No AudioSource component was attached, cannot play audio.");
                return;
            }

            if (audioSrc.isPlaying) {
                return;
            }

            AudioClip audioClip = GetAudioClip(type);
            if (audioClip == null) {
                Logger.Error(string.Format("No AudioClip found for type: {0} and filename '{1}'. Wrong filename?", type.ToString(), audioClips[type].filename));
                return;
            }

            audioSrc.spatialBlend = spatialBlend;
            audioSrc.PlayOneShot(audioClip, 1.0f);
        } catch (Exception e) {
            Logger.Error(String.Format("Error with playing audio file of type: {0} from source {1}\nError message: {2}", type.ToString(), audioSourceObject.ToString(), e.Message));
        }
    }

    private AudioSource GetAudioSource(GameObject sourceObject, AudioClipType type) {
        GameObject src = sourceObject ?? GetDefaultAudioSource(type);
        foreach (AudioSource audioSrc in GameObjectUtility.EnsureComponents<AudioSource>(src, audioClips[type].maxCount)) {
            if (!audioSrc.isPlaying) {
                return audioSrc;
            }
        }
        return null;
    }

    private GameObject GetDefaultAudioSource(AudioClipType type) {
        GameObject source = GameObject.FindWithTag(audioClips[type].sourceTag);
        if (source == null) {
            Logger.Error("Did not find a GameObject tagged with DefaultAudioSource cannot play audio!");
        }
        return source;
    }

    private AudioClip GetAudioClip(AudioClipType type) {
        if (audioClips[type].clip == null) {
            audioClips[type].clip = Resources.Load<AudioClip>(string.Format("{0}{1}", AUDIO_FILE_LOCATION, audioClips[type].filename));
        }
        return audioClips[type].clip;
    }

    private class AudioClipData {

        public string sourceTag;
        public string filename;
        public int maxCount;
        public AudioClip clip;

        public AudioClipData(string sourceTag, string filename, int maxCount) {
            this.sourceTag = sourceTag;
            this.filename = filename;
            this.maxCount = maxCount;
            clip = null;
        }
    }
}
