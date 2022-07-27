using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMembrane : Task {

    ///  <summary>
    ///  Constructor for FinishMembrane task.
    ///  Is not removed when finished and requires previous task completion.
    ///  </summary>
    public FinishMembrane() : base(TaskType.FinishMembrane, true) {

    }

    public override void Subscribe() {

    }

    public override void StartTask() {
        if (!Started) FinishTask();
        base.StartTask();
    }

    public override async void FinishTask() {
        await System.Threading.Tasks.Task.Delay(1000);
        CompleteTask();
        base.FinishTask();
    }
}
