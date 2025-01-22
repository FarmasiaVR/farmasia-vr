using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Used to loop any audio. Change attributes accordingly to customise sound's start, looping, and ending sections.
public class LoopingAudio : MonoBehaviour
{
    public AudioSource audioSource;

    // If you wish to skip initial parts of the audio clip.
    // Also can be set the same as loopStartTime if you don't want a distinct starting sound.
    public float startTime;

    // Which part to loop. Defaults to looping whole audio clip.
    public float loopStartTime;
    public float loopEndTime;

    // Determine distinct part to play last. Defaults to abrupt stop. Smooth ending when soundEndingStart == loopEndTime.
    public float soundEndingStart;
    public float soundEndingStop;

    private bool isLooping = false;
    private bool isStopping = false;

    // Start is called before the first frame update
    void Start()
    {
        if (soundEndingStart == 0)
        {
            soundEndingStart = audioSource.clip.length - 0.001f;
        }

        if (soundEndingStop == 0)
        {
            soundEndingStop = audioSource.clip.length;
        }

        if (loopEndTime == 0)
        {
            loopEndTime = audioSource.clip.length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLooping)
        {
            if (audioSource.time >= loopEndTime)
            {
                audioSource.time = loopStartTime;
            }
        }

        if (isStopping)
        {
            if (audioSource.time < soundEndingStart)
            {
                audioSource.time = soundEndingStart;
            }

            if (audioSource.time >= soundEndingStop)
            {
                audioSource.Stop();
                isStopping = false;
            }
        }
    }

    public void Play()
    {

        if (isStopping)
        {
            audioSource.time = loopStartTime;
            audioSource.Play();
            isLooping = true;
        } else if (!isLooping)
        {
            audioSource.time = startTime;
            audioSource.Play();
            isLooping = true;

        }
        isStopping = false;
    }

    public void Stop()
    {
        isLooping = false;
        isStopping = true;
        audioSource.time = soundEndingStart;
    }
}
