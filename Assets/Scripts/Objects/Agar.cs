using System;
using Codice.CM.WorkspaceServer.Tree.GameUI.Checkin.Updater;
using FarmasiaVR.Legacy;
using JetBrains.Annotations;
using Unity;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Agar : Interactable {

    private DateTime lastTouched;
    private int leftHandTouches;
    private int rightHandTouches;
    private bool leftThumbTouch;
    private bool leftMidFgrTouch;
    private bool rightThumbTouch;
    private bool rightMidFgrTouch;
    //private XRBaseInteractable interactable;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
        leftHandTouches = 0;
        rightHandTouches = 0;
        //interactable = GetComponent<XRBaseInteractable>();
        leftThumbTouch = false;
        leftMidFgrTouch = false;
        rightMidFgrTouch = false;
        rightThumbTouch = false;
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
        checkComplete();

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

        if (checkPokeInteractor(args))
        {
            XRPokeInteractor interactor = (XRPokeInteractor)args.interactorObject;
            handTouches(args, interactor);
        }
        checkComplete();
    }
    private bool checkPokeInteractor(SelectExitEventArgs args)
    {
        if (args.interactableObject is XRPokeInteractor)
        {
            return true;
        }
        return false;
    }

    private void handTouches(SelectExitEventArgs args, XRPokeInteractor interactor)
    {
        if (args.interactorObject.transform.gameObject.tag == "Controller (Left)" && interactor.attachTransform.tag == "PokeAttachMidFgr")
        {
            leftMidFgrTouch = true;
            //leftHandTouches++;
        }
        if (args.interactorObject.transform.gameObject.tag == "Controller (Left)" && interactor.attachTransform.tag == "PokeAttachThumb")
        {
            leftThumbTouch = true;
            //leftHandTouches++;
        }
        if (args.interactorObject.transform.gameObject.tag == "Controller (Right)" && interactor.attachTransform.tag == "PokeAttachMidFgr")
        {
            rightMidFgrTouch = true;
            //rightHandTouches++;
        }
        if (args.interactorObject.transform.gameObject.tag == "Controller (Right)" && interactor.attachTransform.tag == "PokeAttachThumb")
        {
            rightThumbTouch = true;
            //rightHandTouches++;
        }
    }

    private void checkComplete()
    {
        if (leftMidFgrTouch && leftThumbTouch)
        {
            Events.FireEvent(EventType.FingerprintsGivenL);
        }
        if (rightMidFgrTouch && rightThumbTouch)
        {
            Events.FireEvent(EventType.FingerprintsGivenR);
        }
    }
}
