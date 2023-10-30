using UnityEngine;
using UnityEngine.Localization.Settings;

public static class Translator
{
    static LocalSelector localSelector;

    static Translator()
    {
        GameObject localizationManagerObject = GameObject.Find("LocalizationManager");
        Debug.Log("This code is driven");

        if (localizationManagerObject != null)
        {
            localSelector = localizationManagerObject.GetComponent<LocalSelector>();
            Debug.Log("LocalSelector Found in Translator constructor");
        }
    }

    public static string Translate(string table, string key)
    {
        SetLocale();
        string handle = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString(table, key);
        return handle;
    }

    private static void SetLocale()
    {
        if (localSelector == null)
        {
            Debug.Log("Method SetLocale: LocalSelector not found!");
            return;
        }

        int currentLocale = localSelector.FetchCurrentLocale();

        if (UnityEngine.Localization.Settings.LocalizationSettings.SelectedLocale != UnityEngine.Localization.Settings.LocalizationSettings.AvailableLocales.Locales[currentLocale])
        {
            localSelector.ChangeLocale(currentLocale);
        }
    }
}
