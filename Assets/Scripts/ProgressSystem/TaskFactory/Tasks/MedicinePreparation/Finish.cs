using UnityEngine;

public class Finish : TaskBase {

    #region Constructor
    ///  <summary>
    ///  Constructor for Finish task.
    ///  Is not removed when finished and requires previous task completion.
    ///  </summary>
    public Finish() : base(TaskType.Finish, false, true) {
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all tasks are completed, this method is called.
    /// </summary>
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
    #endregion
}
