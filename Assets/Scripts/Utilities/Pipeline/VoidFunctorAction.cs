public class VoidFunctorAction : PipelineAction {

    private Pipeline.VoidFunction functor;

    public VoidFunctorAction(Pipeline.VoidFunction functor) {
        this.functor = functor;
    }

    protected override void UpdateAction(float deltaTime) {
        functor?.Invoke();
        IsDone = true;
    }
}