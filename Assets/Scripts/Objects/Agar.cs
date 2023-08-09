using System;
using FarmasiaVR.Legacy;
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
    private DateTime lastError;
    private WritingType leftHandType;
    private WritingType rightHandType;
    //private XRBaseInteractable interactable;
    [SerializeField]
    GameObject agarPlateLid;
    private Writable writableScript;

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
        leftHandType = WritingType.LeftHand;
        rightHandType = WritingType.RightHand;
        writableScript = agarPlateLid.GetComponent<Writable>();
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
            TimeSpan timeSpan = DateTime.Now - lastError;
            if (timeSpan.Seconds > 1.5f) { 
                Task.CreateTaskMistake(TaskType.Fingerprints, "Kosketuksen tulee olla noin 5 sekuntia", 1);
                lastError = DateTime.Now;
            }
            
        }

        XRPokeInteractor interactor = (XRPokeInteractor)args.interactorObject;
        handTouches(args, interactor);
       
        checkComplete();
    }

    private void handTouches(SelectExitEventArgs args, XRPokeInteractor interactor)
    {
        if (args.interactorObject.transform.parent.tag == "Controller (Left)" && interactor.attachTransform.tag == "PokeAttachMidFgr" && handCheck(args))
        {
            leftMidFgrTouch = true;
            //leftHandTouches++;
        }
        if (args.interactorObject.transform.parent.tag == "Controller (Left)" && interactor.attachTransform.tag == "PokeAttachThumb" && handCheck(args))
        {
            leftThumbTouch = true;
            //leftHandTouches++;
        }
        if (args.interactorObject.transform.parent.tag == "Controller (Right)" && interactor.attachTransform.tag == "PokeAttachMidFgr" && handCheck(args))
        {
            rightMidFgrTouch = true;
            //rightHandTouches++;
        }
        if (args.interactorObject.transform.parent.tag == "Controller (Right)" && interactor.attachTransform.tag == "PokeAttachThumb" && handCheck(args))
        {
            rightThumbTouch = true;
            //rightHandTouches++;
        }
    }

    private bool handCheck(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.parent.tag == "Controller (Left)" && writableScript.GetWritingsInOrder().Contains(leftHandType))
        {
            return true;
        }
        else if (args.interactorObject.transform.parent.tag == "Controller (Right)" && writableScript.GetWritingsInOrder().Contains(rightHandType))
        {
            return true;
        }
        else
        {
            Task.CreateTaskMistake(TaskType.Fingerprints, "Väärällä kädellä otetut sormenjäljet", 1);
            return false;
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
