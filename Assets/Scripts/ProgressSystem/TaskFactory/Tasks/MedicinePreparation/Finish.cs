public class Finish : TaskBase {

    public Finish() : base(TaskType.Finish, false, true) {
    }

    public override void FinishTask() {
        UISystem.Instance.CreatePopup("Congratulations!\nAll tasks finished", MessageType.Done);
        base.FinishTask();
    }

    public override string GetDescription() {
        return base.GetDescription();
    }

    public override string GetHint() {
        return base.GetHint();
    }

    public override void Subscribe() {
        base.Subscribe();
    }
}
