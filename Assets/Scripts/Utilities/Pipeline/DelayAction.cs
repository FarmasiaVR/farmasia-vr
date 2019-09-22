public class DelayAction : PipelineAction {

    #region fields
    private float delaySec;
    private float currentSec;
    #endregion

    public DelayAction(float delaySec) {
        this.delaySec = delaySec;
    }

    protected override void UpdateAction(float deltaTime) {
        currentSec += deltaTime;
        if (currentSec >= delaySec) {
            IsDone = true;
        }
    }
}