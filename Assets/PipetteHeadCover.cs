using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipetteHeadCover : GeneralItem {
    private Cover cover;

    [SerializeField] 
    private GameObject pipette;

    protected override void Start() {
        base.Start();
        cover = gameObject.GetComponent<Cover>();
        Type.On(InteractableType.Interactable);
        cover.DisableOpeningSpots();

        cover.OnCoverOpen = (hand) => {
            //Events.FireEvent(EventType.PipetteCoverOpened, CallbackData.Object(this));
            EnablePipette(hand);
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

    public void EnablePipette(Hand hand) {
        pipette.transform.SetParent(null);
        pipette.SetActive(true);
        gameObject.SetActive(false);
        hand.Uninteract();

        var pipetteComponent = pipette.GetComponent<PipetteContainer>();
        hand.Connector.ConnectItem(pipetteComponent);
        hand.interactedInteractable = pipetteComponent;
        pipetteComponent.State.On(InteractState.Grabbed);
        pipetteComponent.OnGrabStart(hand);
    }
}