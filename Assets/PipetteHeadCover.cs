using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipetteHeadCover : GeneralItem {
    private Cover cover;

    [SerializeField] 
    private PipetteContainer pipette;

    protected override void Start() {
        base.Start();
        cover = gameObject.GetComponent<Cover>();
        Type.On(InteractableType.Interactable);
        cover.DisableOpeningSpots();

        cover.OnCoverOpen = (hand) => {
            //Events.FireEvent(EventType.PipetteCoverOpened, CallbackData.Object(this));
            base.StartCoroutine(EnablePipette(hand));
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

    IEnumerator EnablePipette(Hand hand) {
        pipette.transform.SetParent(null);
        pipette.gameObject.SetActive(true);

        gameObject.SetActive(false);

        yield return null;
        
        hand.Uninteract();

        Logger.Print("Hand grabbing " + pipette);

        hand.InteractWith(pipette);

        yield break;
    }
}