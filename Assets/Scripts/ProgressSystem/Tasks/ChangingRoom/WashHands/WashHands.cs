using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WashHands : Task {

    public enum Conditions { HandsWashed }

    public enum HandsState { dirty, soapy, wet, clean, cleanest };
    public HandsState handState = HandsState.dirty;

    public WashHands(TaskType taskType) : base(taskType, false) {
        SetCheckAll(false);
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

    private void HandsTouched(CallbackData data) {

        var liquidUsed = (data.DataObject as HandWashingLiquids);
        if (liquidUsed == null) return;

        if (liquidUsed.type == "Soap") {
            handState = HandsState.soapy;
            Debug.Log("HansState : " + handState);
            // ok
        }

        if (liquidUsed.type == "Water" && handState != HandsState.soapy) {
            handState = HandsState.wet;
            Debug.Log("HansState : " + handState);
            // mistake, minus points?
        }
            
        if (liquidUsed.type == "Water" && handState == HandsState.soapy) {
            handState = HandsState.clean;
            Debug.Log("HansState : " + handState);
            // ok
        }

        if (liquidUsed.type == "HandSanitizer" && handState != HandsState.clean) {
            handState = HandsState.dirty;
            Debug.Log("HansState : " + handState);
            // mistake, minus points?
        }

        // Conditions for completting the washing hands
        if (liquidUsed.type == "HandSanitizer" && handState == HandsState.clean) {
            EnableCondition(Conditions.HandsWashed);
            handState = HandsState.cleanest;
            Debug.Log("HansState : " + handState);
            CompleteTask();
        }
    }

}