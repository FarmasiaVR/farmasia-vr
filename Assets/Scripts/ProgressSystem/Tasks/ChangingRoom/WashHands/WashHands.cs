using System;
using UnityEngine;

public class WashHands : Task {

    public enum Conditions { HandsWashed }

    public enum HandsState { dirty, soapy, wet, clean, cleanest };
    public HandsState handState = HandsState.dirty;

    public WashHands(TaskType taskType) : base(taskType, false) {
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }


    public override void Subscribe() {
        base.SubscribeEvent(HandsTouched, EventType.WashingHands);
    }

    /*
    private void ChangeHandState(HandsState handState) {
        this.handState = handState;
        Debug.Log("HansState : " + handState);
    }
    */

    // Track progress of washing hands
    private void HandsTouched(CallbackData data) {

        var liquidUsed = (data.DataObject as HandWashingLiquid);
        if (liquidUsed == null) return;
        Debug.Log("Liquid: " + liquidUsed.type);

        if (liquidUsed.type == "Soap") {
            handState = HandsState.soapy;
            Debug.Log("HandsState : " + handState);
            // ok
        }

        if (liquidUsed.type == "Water" && handState != HandsState.soapy) {
            handState = HandsState.wet;
            Debug.Log("HandsState : " + handState);
            // mistake, minus points?
        }
            
        if (liquidUsed.type == "Water" && handState == HandsState.soapy) {
            handState = HandsState.clean;
            Debug.Log("HandsState : " + handState);
            // ok
        }

        if (liquidUsed.type == "HandSanitizer" && handState != HandsState.clean) {
            handState = HandsState.dirty;
            Debug.Log("HandsState : " + handState);
            // mistake, minus points?
        }

        // Conditions for completting the washing hands
        if (liquidUsed.type == "HandSanitizer" && handState == HandsState.clean) {
            EnableCondition(Conditions.HandsWashed);
            handState = HandsState.cleanest;
            Debug.Log("HansdState : " + handState);
            CompleteTask();
        }
    }

}