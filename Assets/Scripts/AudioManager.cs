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
        audioClips.Add(AudioClipType.LockedItem, new AudioClipData("LockedItemAudioSource", "Item_locked"));
        audioClips.Add(AudioClipType.MistakeMessage, new AudioClipData("MistakeAudioSource", "Mistake_message"));
        audioClips.Add(AudioClipType.TaskCompletedBeep, new AudioClipData("TaskCompletedAudioSource", "Task_completed_beep1"));
        audioClips.Add(AudioClipType.MinusPoint, new AudioClipData("MinusPointAudioSource", "Minus_point"));
    }

    public void Play(AudioClipType type, GameObject audioSourceObject = null, float spatialBlend = 0) {
        AudioSource audioSrc = GetAudioSource(audioSourceObject, type);
        AudioClip audioClip = GetAudioClip(type);
        if (audioSrc == null || audioClip == null || audioSrc.isPlaying) {
            return;
        }

        audioSrc.spatialBlend = spatialBlend;
        audioSrc.PlayOneShot(audioClip, 1.0f);
    }

    private AudioSource GetAudioSource(GameObject sourceObject, AudioClipType type) {
        GameObject src = sourceObject ?? GetDefaultAudioSource(type);
        AudioSource audioSrc = GameObjectUtility.EnsureComponent<AudioSource>(src);
        return audioSrc;
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
            audioClips[type].clip = Resources.Load<AudioClip>(AUDIO_FILE_LOCATION + audioClips[type].filename);
        }
        return audioClips[type].clip;
    }

    private class AudioClipData {

        public string sourceTag;
        public string filename;
        public AudioClip clip;

        public AudioClipData(string sourceTag, string filename) {
            this.sourceTag = sourceTag;
            this.filename = filename;
            clip = null;
        }
    }
}
