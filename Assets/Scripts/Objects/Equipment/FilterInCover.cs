using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterInCover : ReceiverItem {
    public LiquidContainer Container { get; private set; }

    [SerializeField]
    private Cover cover;

    [SerializeField]
    private List<FilterPart> FilterParts;

    private bool coverOn;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        Container = LiquidContainer.FindLiquidContainer(transform);
        objectType = ObjectType.FilterInCover;
        Type.On(InteractableType.Interactable);
        cover.DisableOpeningSpots();

        cover.OnCoverOpen = (hand) => {
            Events.FireEvent(EventType.FilterCoverOpened, CallbackData.Object(this));
            EnableAssemblyFilterParts();
        };
    }
    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        cover.EnableOpeningSpots();
    }

    public void OpenCoverXR()
    {
        Events.FireEvent(EventType.FilterCoverOpened, CallbackData.Object(this));
        EnableAssemblyFilterParts();
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabEnd(hand);
        cover.DisableOpeningSpots();
    }
    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);
        cover.OpenCoverWithHand(hand);

    }
    public void EnableAssemblyFilterParts() {
        foreach (FilterPart part in FilterParts) {
            part.enabled = true;
        }

        AfterRelease = (interactable) => {
            Events.FireEvent(EventType.FilterDissassembled, CallbackData.Object((this, interactable)));
        };
        AfterConnect = (interactable) => {
            Events.FireEvent(EventType.FilterAssembled, CallbackData.Object(new List<GeneralItem>() { this, interactable as GeneralItem }));
        };
    }

}


