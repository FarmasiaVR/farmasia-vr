using System.Threading.Tasks;
using UnityEngine.Localization.Settings;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public static class Translator
{
    async public static Task<string> Translate(string table, string key)
    {
        var handle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table, key);

        await handle.Task;

        return handle.Result;
    }
}