using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalSelector : MonoBehaviour
{
    private bool active = false;
    private int currentLocale;

    public int FetchCurrentLocale() { return currentLocale; }
    public void ChangeLocale(int localeID)
    {
        if (active) { return; }
        // StartCoroutine(SetLocale(localeID));
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        TaskConfig.reInitDictionary(); //TaskConfig contains most of the translations so reInit it to get the updated texts...
    }

    //this is BAD but works, will fix soon TM 
    private void Update()
    {
        TaskConfig.reInitDictionary();
    }
   

    IEnumerator SetLocale(int _localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        if (_localeID >= 0 && _localeID < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            active = true;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
            currentLocale = _localeID;
            Debug.Log(currentLocale);
            Debug.Log(_localeID);

            active = false;
        }
        else
        {
            Debug.LogWarning("Invalid locale ID provided.");
        }
        
    }
}
