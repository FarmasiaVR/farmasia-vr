using UnityEngine.Localization.Settings;

public static class Translater
{
    public static void Translate(string table, string key, System.Action<string> callback)
    {
        var handle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table, key);

        handle.Completed += (operationHandle) =>
        {
            callback(operationHandle.Result);
        };
    }
}
