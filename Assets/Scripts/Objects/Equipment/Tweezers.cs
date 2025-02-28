﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweezers : ReceiverItem {

    [SerializeField]
    private Cover cover;
    public HandCollider TweezerCollider { get; private set; }

    //Keeps track whether filter half is on range of tweezers grab range
    private bool filterCanBeGrabbed = false;

    //Keeps track whether filter half is grabbed
    private bool filterIsGrabbed = false;

    //Grabbed filter half
    private GameObject filter;

    public FilterHalf[] filterHalves;

    private bool coverOn;

    private Transform originalParent;
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        objectType = ObjectType.Tweezers;     
        Type.On(InteractableType.Interactable);
        cover.DisableOpeningSpots();
        var pos = GetComponent<SphereCollider>().center;
        AfterConnect = (Interactable) => {
            Interactable.transform.Rotate(new Vector3(0, 0, 90));
        };

        AfterRelease = (interactable) => {
            interactable.transform.position = transform.TransformPoint(pos);
        };

        cover.OnCoverOpen = (hand) => {
            Events.FireEvent(EventType.TweezersCoverOpened, CallbackData.Object(this));
        };
    }
    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        cover.EnableOpeningSpots();
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabEnd(hand);
        cover.DisableOpeningSpots();
    }

    public void openCoverXR()
    {
        Events.FireEvent(EventType.TweezersCoverOpened, CallbackData.Object(this));
    }

    
    //Checks whether filter can be grabbed, if true, disables filter halfs rigidBody
    public void grabFilterWithTweezers()
    {

        filterHalves = FindObjectsOfType<FilterHalf>();
        UnityEngine.Debug.Log("Found FilterHalves: " + filterHalves.Length);

        if (!filterCanBeGrabbed || cover.CoverOn) return;

        filter.GetComponent<Rigidbody>().isKinematic = true;
        filter.GetComponent<Rigidbody>().detectCollisions = false;

        originalParent = transform.parent;
        filter.transform.SetParent(transform);
        filterIsGrabbed = true;
    }

    //Enables the filter halfs rigidBody once its let go, sets tweezers grab state to false and resets internal filter object
    public void letGoOfFilterWithTweezers()
    {
        if (filterIsGrabbed)
        {
            filter.GetComponent<Rigidbody>().isKinematic = false;
            filter.GetComponent<Rigidbody>().detectCollisions = true;
            
            transform.parent = originalParent;
            
            filterIsGrabbed = false;
            filter = null;
        }
        letGoOverride();
    }

    // bug fix code by group 6. Filter half would sometimes get stuck on the tweezers; this was due to the filter's "isKinematic" toggle staying on, even after the "letGoOfFilterWithTweezers() was called.
    public void letGoOverride()
    {
         foreach (FilterHalf f in filterHalves)
        {
            f.GetComponent<Rigidbody>().isKinematic = false;
        }
        
    }


    //Sets filterCanBeGrabbed to true and private filter object if collided object is a filter half (fyi filter halfs tag is set to Interactable at runtime)
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FilterHalf>())
        {
            filterCanBeGrabbed = true;
            filter = other.gameObject;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        filterCanBeGrabbed = false;   
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);
        cover.OpenCoverWithHand(hand);
        coverOn = cover.CoverOn;
        if(VRInput.GetControlUp(base.grabbingHand.HandType, Controls.GrabInteract)) {
            if (!SlotOccupied) return;
            Interactable obj = ConnectedItem;
            ConnectedItem.ResetItem();
            Disconnect(hand, ConnectedItem);
            AfterRelease(obj);
        }
    }

    protected override bool WillConnect() {
        if (grabbingHand == null) {
            return false;
        }
        return coverOn == false && VRInput.GetControlDown(base.grabbingHand.HandType, Controls.GrabInteract) && base.WillConnect();
    }
}
