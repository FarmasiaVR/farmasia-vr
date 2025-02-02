using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRHandWashingLiquid : MonoBehaviour
{
    public string type;

    public float runningTime = 5f;
    private float currentRunningTime;

    AudioSource handWashingSound;

    private bool running = false;

    new ParticleSystem particleSystem;

    private XRBaseInteractable interactable;

    private HandWashingLiquid legacyObject;
    private Collider dispenserCollider;

    void Start()
    {
        handWashingSound = gameObject.GetComponent<AudioSource>();
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        currentRunningTime = runningTime;

    }

    public void Interact(Collider other)
    {
        // This event should be ignored if the object interacting isn't the player.
        if (!(other.CompareTag("Controller (Left)") | other.CompareTag("Controller (Right)"))) return;
        // Should not run if the game is completed.
        TaskType currentTask = G.Instance.Progress.CurrentPackage.CurrentTask.TaskType;
        if (type.Equals("Water") || (currentTask == TaskType.WashHandsInChangingRoom || currentTask == TaskType.WashHandsInPreperationRoom))
        {
            if (!running)
            {
                running = true;
                PlayFX();
            }
        }
        UpdateLegacyObject();
        Events.FireEvent(EventType.WashingHands, CallbackData.Object(legacyObject));
    }

    public void PlayFX()
    {
        if (particleSystem != null) particleSystem.Play();
        if (handWashingSound != null) handWashingSound.Play();
    }

    void TimerCountdown()
    {
        currentRunningTime = currentRunningTime - Time.deltaTime;
        if (currentRunningTime <= 0)
        {
            running = false;
            currentRunningTime = runningTime;
        }
    }

    public bool IsRunning()
    {
        return running;
    }

    private void UpdateLegacyObject()
    {
        legacyObject = new HandWashingLiquid();
        legacyObject.type = type;
        legacyObject.runningTime = runningTime;
    }

    void Update()
    {
        if (running)
        {
            TimerCountdown();
        }
    }
}
