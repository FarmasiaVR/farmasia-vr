using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ControlTutorialManager : MonoBehaviour
{
    private TaskManager taskManager;

    /// <summary>
    /// This a scene manager used for the controls tutorial scene. This is used to keep track of the progression in the scene.
    /// </summary>
    private void Start()
    {
        taskManager = GetComponent<TaskManager>();
    }
    public void CheckBottleFill(LiquidContainer container)
    {
        if (container.Amount >= 30000)
        {
            taskManager.CompleteTask("med");
        }
    }

    public void CheckFingerprintTime(double time)
    {
        if (time >= 4.5f & time <= 5.5f)
        {
            taskManager.CompleteTask("fingerprint");
        }
    }

    public void GeneralMistake(string key, int penalty){
        var localizedString = new LocalizedString("ControlsTutorial", key);
        localizedString.StringChanged += (localizedText) => {
            taskManager.GenerateGeneralMistake(localizedText, penalty);
        };}
}
