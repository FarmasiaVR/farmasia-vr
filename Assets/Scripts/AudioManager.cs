using UnityEngine;

public enum AudioClipType {
    LockedItem,
    MistakeMessage,
    TaskCompletedBeep
}

public class AudioManager {

    private const string AUDIO_FILE_LOCATION = "Audio/Clips/";
    
    #region Public methods
    public void Play(AudioClipType type, GameObject audioSourceObject = null, float spatialBlend = 0) {
        AudioSource audioSrc = GetAudioSource(audioSourceObject);
        if (audioSrc != null) {
            audioSrc.spatialBlend = spatialBlend;
            PlayAudioClip(audioSrc, GetAudioClip(type));
        }
    }
    #endregion

    private AudioSource GetAudioSource(GameObject sourceObject) {
        GameObject src = sourceObject ?? GetDefaultAudioSource();
        AudioSource audioSrc = src?.GetComponent<AudioSource>() ?? src?.AddComponent<AudioSource>();
        return audioSrc;
    }

    private GameObject GetDefaultAudioSource() {
        GameObject source = GameObject.FindWithTag("DefaultAudioSource");
        if (source == null) {
            Logger.Error("Did not find a GameObject tagged with DefaultAudioSource cannot play audio!");
        }
        return source;
    }

    private AudioClip GetAudioClip(AudioClipType type) {
        switch (type) {
            case AudioClipType.LockedItem:
                return Resources.Load<AudioClip>(AUDIO_FILE_LOCATION + "Item_locked");
            case AudioClipType.MistakeMessage:
                return Resources.Load<AudioClip>(AUDIO_FILE_LOCATION + "Mistake_message");
            case AudioClipType.TaskCompletedBeep:
                return Resources.Load<AudioClip>(AUDIO_FILE_LOCATION + "Task_completed_beep1");
            default:
                return null;
        }
    }

    private void PlayAudioClip(AudioSource source, AudioClip clip) {
        if (clip != null) {
            source.PlayOneShot(clip, 1.0f);
        } else {
            Logger.Error("Did not find sound clip");
        }
    }
}
