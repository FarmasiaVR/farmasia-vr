using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cover : MonoBehaviour
{
    private Hand other;
    private HandCollider otherCollider;
    private bool coverOn;
    public bool CoverOn {
        get { return coverOn; }
    }
    [SerializeField]
    private GameObject coverGameObject;
    [SerializeField]
    private Interactable rightOpeningSpot;
    [SerializeField]
    private Interactable wrongOpeningSpot;

    public Action<Hand> OpenCoverWithHand;

    public Action<Hand> OnCoverOpen = (hand) => { };    
    public GameObject itemInside;
    // Start is called before the first frame update
    protected void Start() {
        coverOn = true;
        OpenCoverWithHand = OpenCoverWithButtonPress;
        DisableOpeningSpots();
    }

    private void OpenCoverWithButtonPress(Hand hand) {
        if (!coverOn) return;
        other = hand.Other;
        otherCollider = other.HandCollider;
        bool openCover = VRInput.GetControlDown(other.HandType, Controls.GrabInteract);

        if (openCover && ClosestEqualsWrongOpeningSpot()) {
            Events.FireEvent(EventType.WrongSpotOpened, CallbackData.Object(this));
            OpenCover(hand);
        } 
        if (openCover && ClosestEqualsRightOpeningSpot()) {
            OpenCover(hand);
        }
    }

    //Called when player grabs on to the wrong opening spot whilst holding the object,
    //disabling / removing cover and calling an event informing that it was opened from wrong spot
    public void OpenCoverWrongSpotXR()
    {
        if (!coverOn) return;
        coverOn = false;
        ActivatingItemInside();
        coverGameObject.SetActive(false);
        Events.FireEvent(EventType.WrongSpotOpened, CallbackData.Object(this));
        DisableOpeningSpots();
    }

    //Called when player grabs on to the correct opening spot whilst holding the object,
    //disabling / removing cover
    public void OpenCoverRightSpotXR()
    {
        if (!coverOn) return;
        coverOn = false;
        ActivatingItemInside();
        coverGameObject.SetActive(false);
        DisableOpeningSpots();
    }

    private void OpenCoverWithPull(Hand hand) {
        if (!coverOn) return;
        other = hand.Other;
        otherCollider = other.HandCollider;
        bool openCover = Vector3.Distance(other.transform.position, transform.position) > 0.1f;

        if (openCover && ClosestEqualsWrongOpeningSpot()) {
            Events.FireEvent(EventType.WrongSpotOpened, CallbackData.Object(this));
            OpenCover(hand);
        }
        if (openCover && ClosestEqualsRightOpeningSpot()) {
            OpenCover(hand);
        }
    }

    public void OpenCover(Hand hand) {        
        coverOn = false;        
        ActivatingItemInside();
        coverGameObject.SetActive(false);
        OnCoverOpen(hand);
    }

    private bool ClosestEqualsWrongOpeningSpot() {
        var closest = otherCollider.GetClosestInteractable();
        return (GameObject.ReferenceEquals(wrongOpeningSpot, closest));
    }
    private bool ClosestEqualsRightOpeningSpot() {
        var closest = otherCollider.GetClosestInteractable();
        return (GameObject.ReferenceEquals(rightOpeningSpot, closest));
    }

    //Called when player loads in to scene or when lets go of an object with a cover on
    //This makes covers unopenable unless player has grabbed the object
    public void DisableOpeningSpots() {
        rightOpeningSpot?.transform.gameObject.SetActive(false);
        wrongOpeningSpot?.transform.gameObject.SetActive(false);
    }

    //Enables opening spots when player grabs on to the object
    public void EnableOpeningSpots() {
        if (!coverOn) return;
        rightOpeningSpot?.transform.gameObject.SetActive(true);
        wrongOpeningSpot?.transform.gameObject.SetActive(true);
    }

    public void ActivatingItemInside(){
        Debug.Log("Doro this code is runing");
        if (itemInside == null) return;
        
        itemInside.SetActive(true);
        itemInside.transform.SetParent(null);
        
    }
}
