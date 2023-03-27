using System;
using FarmasiaVR.Legacy;
using JetBrains.Annotations;
using Unity;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Agar : Interactable {

    private DateTime lastTouched;
    private int leftHandTouches;
    private int rightHandTouches;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
        leftHandTouches = 0;
        rightHandTouches = 0;
    }

    public override void Interact(Hand hand) {
        lastTouched = DateTime.Now;
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
        string handIs = hand.HandType.ToString();
        TimeSpan time = DateTime.Now - lastTouched;
        Logger.Print("Fingerprints given for " + time + " seconds");
        base.Interact(hand);
        if (handIs.Equals("LeftHand")) {
            leftHandTouches++;
        } else {
            rightHandTouches++;
        }
        if (time.Seconds < 4.0f || time.Seconds > 6.0f) {
            Task.CreateTaskMistake(TaskType.Fingerprints, "Kosketuksen tulee olla noin 5 sekuntia", 1);
        }
        if (leftHandTouches >= 2) {
            Events.FireEvent(EventType.FingerprintsGivenL);
        } else if (rightHandTouches >= 2) {
            Events.FireEvent(EventType.FingerprintsGivenR);
        }


        

    }

  


    public void startTakingFingerPrints()
    {
        lastTouched = DateTime.Now;
    }


    public void stopTakingFingerPrints(SelectExitEventArgs args)
    {
        TimeSpan time = DateTime.Now - lastTouched;
        Debug.Log("Fingerprints given for " + time + " seconds");

        if (time.Seconds < 4.0f || time.Seconds > 6.0f)
        {
            Task.CreateTaskMistake(TaskType.Fingerprints, "Kosketuksen tulee olla noin 5 sekuntia", 1);
        }

        if (args.interactorObject.transform.gameObject.tag == "Controller (Left)")
        {
            leftHandTouches++;
        }
        else if (args.interactorObject.transform.gameObject.tag == "Controller (Right)")
        {
           rightHandTouches++;
        }

        if (leftHandTouches >= 2)
        {
            Events.FireEvent(EventType.FingerprintsGivenL);
        }
        else if (rightHandTouches >= 2)
        {
            Events.FireEvent(EventType.FingerprintsGivenR);
        }
    }
}
