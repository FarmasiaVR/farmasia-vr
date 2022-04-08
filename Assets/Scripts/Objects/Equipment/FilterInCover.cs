using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterInCover : GeneralItem {
    [SerializeField]
    private Cover cover;
    [SerializeField]
    private GameObject assemblyFilterParts;
    [SerializeField]
    private GameObject filterAndCoverModel;
    [SerializeField]
    private Interactable filterBase;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();        
        objectType = ObjectType.FilterInCover;
        Type.On(InteractableType.Interactable);
        cover.DisableOpeningSpots();

        cover.OnCoverOpen = (hand) => {
            Events.FireEvent(EventType.FilterCoverOpened, CallbackData.Object(this));
            EnableAssemblyFilterParts(hand);
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
    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);
        cover.OpenCoverWithHand(hand);
    }
    public void EnableAssemblyFilterParts(Hand hand) {
        assemblyFilterParts.transform.SetParent(null);
        assemblyFilterParts.SetActive(true);
        filterAndCoverModel.SetActive(false);
        hand.Uninteract();
        hand.Connector.ConnectItem(filterBase);
        hand.Connector.GrabbedInteractable = filterBase;
        hand.interactedInteractable = filterBase;
        filterBase.State.On(InteractState.Grabbed);
        filterBase.OnGrabStart(hand);
        gameObject.SetActive(false);
    }

}


