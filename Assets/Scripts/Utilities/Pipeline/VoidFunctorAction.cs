public class VoidFunctorAction : PipelineAction {

    #region fields
    private Pipeline.VoidFunction functor;
    #endregion

    public VoidFunctorAction(Pipeline.VoidFunction functor) {
        this.functor = functor;
    }

    protected override void UpdateAction(float deltaTime) {
        functor?.Invoke();
        IsDone = true;
    }
}