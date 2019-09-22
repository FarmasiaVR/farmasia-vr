public class DelayAction : PipelineAction {

    private float delaySec;
    private float currentSec;

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