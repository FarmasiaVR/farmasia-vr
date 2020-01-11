/// <summary>
/// A Singleton for accessing global systems.
/// Source: https://csharpindepth.com/Articles/Singleton
/// </summary>
public sealed class G {

    #region fields
    private static readonly G instance = new G();
    public static G Instance { get => instance; }

    public AudioManager Audio { get; }
    public ProgressManager Progress { get; private set; }
    public PipelineManager Pipeline { get; }
    #endregion

    static G() {}

    private G() {
        Audio = new AudioManager();
        Pipeline = new PipelineManager();
        Progress = new ProgressManager(false);
    }

    public void ResetProgressManager() {
        Logger.Warning("Resetting ProgressManager");
        Progress = new ProgressManager(false);
    }

    public void Update(float deltaTime) {
        Pipeline.Update(deltaTime);
    }
}