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
    private GameObject cover;
    [SerializeField]
    private Interactable rightOpeningSpot;
    [SerializeField]
    private Interactable wrongOpeningSpot;

    // Start is called before the first frame update
    protected void Start()
    {
        coverOn = true;        
    }
    public void OpenCoverWithButtonPress(Hand hand) {
        other = hand.Other;
        otherCollider = other.HandCollider;
        bool openCover = VRInput.GetControlDown(other.HandType, Controls.TakeMedicine);

        if (openCover && coverOn && (GameObject.ReferenceEquals(rightOpeningSpot, otherCollider.GetClosestInteractable()) || GameObject.ReferenceEquals(wrongOpeningSpot, otherCollider.GetClosestInteractable()))) {
            coverOn = false;
            cover.SetActive(false);
        }

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
