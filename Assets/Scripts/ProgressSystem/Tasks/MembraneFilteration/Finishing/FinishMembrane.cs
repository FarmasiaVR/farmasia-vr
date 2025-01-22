using FarmasiaVR.Legacy;

public class FinishMembrane : Task {

    public FinishMembrane() : base(TaskType.FinishMembrane, true) {

    }

    public override void Subscribe() {

    }

    public override void StartTask() {
        if (!Started) {
            FinishTask();
        }
        base.StartTask();
    }

    public override async void FinishTask() {
        await System.Threading.Tasks.Task.Delay(1000);
        CompleteTask();
        base.FinishTask();
    }
}
