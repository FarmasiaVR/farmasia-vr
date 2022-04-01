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

    // Start is called before the first frame update
    protected void Start() {
        coverOn = true;
        OpenCoverWithHand = OpenCoverWithButtonPress;
    }

    private void OpenCoverWithButtonPress(Hand hand) {
        if (!coverOn) return;
        other = hand.Other;
        otherCollider = other.HandCollider;
        bool openCover = VRInput.GetControlDown(other.HandType, Controls.TakeMedicine);

        if (openCover && ClosestEqualsOpeningSpot()) {
            OpenCover();
        }
    }

    private void OpenCoverWithPull(Hand hand) {
        if (!coverOn) return;
        other = hand.Other;
        otherCollider = other.HandCollider;
        bool openCover = Vector3.Distance(other.transform.position, transform.position) > 0.1f;

        if (openCover && ClosestEqualsOpeningSpot()) {
            OpenCover();
        }
    }

    public void OpenCover() {
        coverOn = false;
        coverGameObject.SetActive(false);
    }

    private bool ClosestEqualsOpeningSpot() {
        var closest = otherCollider.GetClosestInteractable();
        return (GameObject.ReferenceEquals(rightOpeningSpot, closest) 
            || GameObject.ReferenceEquals(wrongOpeningSpot, closest));
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
