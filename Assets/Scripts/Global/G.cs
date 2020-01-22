using UnityEngine;
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

    private SceneScript sceneScript;
    public SceneScript Scene {
        get {
            if (sceneScript == null) {
                sceneScript = GameObject.FindObjectOfType<SceneScript>();
            }
            return sceneScript;
        }
    }
    #endregion

    static G() {
        CheckConfiguration();
    }

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

    private static void CheckConfiguration() {
#if UNITY_VRCOMPUTER
#elif UNITY_NONVRCOMPUTER
        Logger.Warning("Using a NON VR system. Building on this machine will not work. If you want to build, change the VR configuration variable from UNITY_NONVRCOMPUTER to UNITY_VRCOMPUTER in the csc.rsp file.");
#else
        throw new System.Exception("No VR configuration variable set. Check README for instructions.");
#endif
    }
}