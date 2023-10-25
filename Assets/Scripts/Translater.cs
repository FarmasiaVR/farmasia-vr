using System.Threading.Tasks;
using UnityEngine.Localization.Settings;

public static class Translater
{
    async public static Task<string> Translate(string table, string key)
    {
        var handle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table, key);

        await handle.Task;

        return handle.Result;
    }
}
