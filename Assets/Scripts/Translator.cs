using System.Threading.Tasks;
using UnityEngine.Localization.Settings;

public static class Translator
{
    public static string Translate(string table, string key)
    {
        return Task.Run(async () =>
        {
            var handle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table, key);
            await handle.Task;
            return handle.Result;
        }).Result;
    }
}