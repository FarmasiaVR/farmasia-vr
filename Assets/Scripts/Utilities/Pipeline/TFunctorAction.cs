public class TFunctorAction<T> : PipelineAction {

    #region fields
    private Pipeline.TFunction<T> functor;
    private Pipeline.GetTFunction<T> argGetter;
    #endregion

    public TFunctorAction(Pipeline.TFunction<T> functor, Pipeline.GetTFunction<T> argGetter) {
        this.functor = functor;
        this.argGetter = argGetter;
    }

    protected override void UpdateAction(float deltaTime) {
        if (argGetter != null) {
            functor?.Invoke(argGetter.Invoke());
        }
        IsDone = true;
    }
}
