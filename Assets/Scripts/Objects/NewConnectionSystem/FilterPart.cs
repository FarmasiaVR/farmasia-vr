using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterPart : ReceiverItem
{
    public LiquidContainer Container { get; private set; }

    protected override void Start() {
        base.Start();
        Container = LiquidContainer.FindLiquidContainer(transform);

        AfterRelease = (interactable) => {
            Events.FireEvent(EventType.FilterDissassembled, CallbackData.Object((this, interactable)));
        };
        AfterConnect = (interactable) => {
            Events.FireEvent(EventType.FilterAssembled, CallbackData.Object(new List<GeneralItem>() { this, interactable as GeneralItem }));
        };
    }

    public override void ResetItem() {
        base.ResetItem();

        gameObject.AddComponent<Rigidbody>();
        RigidbodyContainer = new RigidbodyContainer(this);
    }

}
