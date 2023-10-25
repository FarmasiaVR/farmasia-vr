using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine.Localization.Settings;
using UnityEngine;

public static class Translator
{
    public static string Translate(string table, string key)
    {
        string handle = LocalizationSettings.StringDatabase.GetLocalizedString(table, key);
        return handle;
    }
}