using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweezers : GeneralItem {

    private Hand other;
    private HandCollider otherCollider;
    private bool coverOn;
    [SerializeField]
    private GameObject cover;
    [SerializeField]
    private Interactable rightOpeningSpot;
    [SerializeField]
    private Interactable wrongOpeningSpot;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        objectType = ObjectType.Tweezers;        
        coverOn = true;
        rightOpeningSpot.transform.gameObject.SetActive(false);
        wrongOpeningSpot.transform.gameObject.SetActive(false);
        Type.On(InteractableType.Interactable);
    }
    public override void OnGrabStart(Hand hand)
    {
        base.OnGrabStart(hand);
        rightOpeningSpot.transform.gameObject.SetActive(true);
        wrongOpeningSpot.transform.gameObject.SetActive(true);
    }

    public override void OnGrabEnd(Hand hand)
    {
        base.OnGrabEnd(hand);
        rightOpeningSpot.transform.gameObject.SetActive(false);
        wrongOpeningSpot.transform.gameObject.SetActive(false);
    }
    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);
        other = hand.Other;        
        otherCollider = other.HandCollider;  
        bool openCover = VRInput.GetControlDown(other.HandType, Controls.TakeMedicine);
        
        if (openCover && coverOn && (GameObject.ReferenceEquals(rightOpeningSpot, otherCollider.GetClosestInteractable()) || GameObject.ReferenceEquals(wrongOpeningSpot, otherCollider.GetClosestInteractable()))) {
            OpenCover();
        }
        
    }
    public void OpenCover() {
        coverOn = false;
        cover.SetActive(false);
    }

}
