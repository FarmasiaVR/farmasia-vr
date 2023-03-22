using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FilterPart : ReceiverItem
{
    public LiquidContainer Container { get; private set; }

    protected override void Start() {
        base.Start();
        Container = LiquidContainer.FindLiquidContainer(transform);

        //2023: to our understanding these events wont do anything or maybe they dò? you can figure it out.
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
