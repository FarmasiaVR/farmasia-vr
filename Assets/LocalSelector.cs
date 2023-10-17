using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalSelector : MonoBehaviour
{
    private bool active = false;
    public void ChangeLocale(int localeID)
    {
        if (active) { return; }
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        if (_localeID >= 0 && _localeID < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            active = true;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
            active = false;
        }
        else
        {
            Debug.LogWarning("Invalid locale ID provided.");
        }
    }
}
