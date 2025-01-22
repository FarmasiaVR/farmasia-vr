public abstract class PipelineAction {

    #region fields
    public bool IsDone { get; protected set; }
    #endregion

    public void Update(float deltaTime) {
        if (!IsDone) {
            UpdateAction(deltaTime);
        }
    }

    protected abstract void UpdateAction(float deltaTime);
}