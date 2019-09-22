/// <summary>
/// A Singleton for accessing global systems.
/// Source: https://csharpindepth.com/Articles/Singleton
/// </summary>
public sealed class G {

    #region fields
    private static readonly G instance = new G();
    public static G Instance { get => instance; }

    public ProgressManager ProgressManager { get; }
    #endregion

    static G() {}

    private G() {
        ProgressManager = new ProgressManager();
    }
}