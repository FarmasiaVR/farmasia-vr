using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    protected void Start() {
        coverOn = true;
        OpenCoverWithHand = OpenCoverWithButtonPress;
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

    public void DisableOpeningSpots() {
        rightOpeningSpot.transform.gameObject.SetActive(false);
        wrongOpeningSpot.transform.gameObject.SetActive(false);
    }

    public void EnableOpeningSpots() {
        rightOpeningSpot.transform.gameObject.SetActive(true);
        wrongOpeningSpot.transform.gameObject.SetActive(true);
    }
}
