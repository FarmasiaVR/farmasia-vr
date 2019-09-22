using System.Collections.Generic;

public class Pipeline {

    public delegate void VoidFunction();
    public delegate void TFunction<T>(T t);
    public delegate T GetTFunction<T>();

    #region fields
    public bool IsDone { get => actions.Count == 0; }
    public int Count { get => actions.Count; }
    private List<PipelineAction> actions;
    #endregion

    public Pipeline() {
        actions = new List<PipelineAction>();
    }

    public Pipeline Func(VoidFunction functor) {
        actions.Add(new VoidFunctorAction(functor));
        return this;
    }

    public Pipeline PreFunc(VoidFunction functor) {
        actions.Insert(0, new VoidFunctorAction(functor));
        return this;
    }

    public Pipeline TFunc<T>(TFunction<T> functor, GetTFunction<T> getter) {
        actions.Add(new TFunctorAction<T>(functor, getter));
        return this;
    }

    public Pipeline PreTFunc<T>(TFunction<T> functor, GetTFunction<T> getter) {
        actions.Insert(0, new TFunctorAction<T>(functor, getter));
        return this;
    }

    public Pipeline Delay(float delaySec) {
        actions.Add(new DelayAction(delaySec));
        return this;
    }

    public Pipeline PreDelay(float delaySec) {
        actions.Insert(0, new DelayAction(delaySec));
        return this;
    }

    public void Update(float deltaTime) {
        if (actions.Count > 0) {
            actions[0].Update(deltaTime);
            if (actions[0].IsDone) {
                actions.RemoveAt(0);
            }
        }
    }
}