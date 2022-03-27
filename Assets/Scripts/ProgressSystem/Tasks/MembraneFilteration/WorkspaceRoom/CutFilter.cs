using UnityEngine;
using System;
using System.Collections.Generic;
public class CutFilter: Task {

    public enum Conditions {
        FilterIsCut
    }


    public CutFilter() : base(TaskType.CutFilter, true, false) {

        SubscribeEvent((Event) => {
            Logger.Print("Filter cutted, nice");
            EnableCondition(Conditions.FilterIsCut);
            CompleteTask();
            Popup(success, MsgType.Done, Points);
        }, EventType.FilterCutted);
    }

    protected override void OnTaskComplete() {
        
    }
}