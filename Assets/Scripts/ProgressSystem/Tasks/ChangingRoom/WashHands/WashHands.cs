using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    private void HandsTouched(CallbackData data) {

        var liquidUsed = (data.DataObject as HandWashingLiquids);
        if (liquidUsed == null) return;

        if (liquidUsed.type == "Soap") {
            handState = HandsState.soapy;
            // ok
        }

        if (liquidUsed.type == "Water" && handState != HandsState.soapy) {
            handState = HandsState.wet;
            // mistake, minus points?
        }
            
        if (liquidUsed.type == "Water" && handState == HandsState.soapy) {
            handState = HandsState.clean;
            // ok
        }

        if (liquidUsed.type == "HandSanitizer" && handState != HandsState.clean) {
            handState = HandsState.dirty;
            // mistake, minus points?
        }

        // Conditions for completting the washing hands
        if (liquidUsed.type == "HandSanitizer" && handState == HandsState.clean) {
            EnableCondition(Conditions.HandsWashed);
            handState = HandsState.cleanest;
            CompleteTask();
        }
    }

}