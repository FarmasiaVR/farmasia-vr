using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Description : MonoBehaviour
{
    private TextMeshPro currentTextField;
    Dictionary<string, string> texts = new Dictionary<string, string>();


    void Start()
    {
        currentTextField = gameObject.GetComponent<TextMeshPro>();


        
        initTextCache();
    }

    void initTextCache()
    {
        texts.Add("PointsRight", Translator.Translate("DressingRoom", "PointsRight").Result);
        texts.Add("SelectionOfTools", Translator.Translate("DressingRoom", "SelectionOfTools").Result);
        texts.Add("Workspace", Translator.Translate("DressingRoom", "Workspace").Result);
        texts.Add("CleaningTheSpace", Translator.Translate("DressingRoom", "CleaningTheSpace").Result);
        texts.Add("DressingRoom", Translator.Translate("DressingRoom", "DressingRoom").Result);
        texts.Add("PreparationArea", Translator.Translate("DressingRoom", "PreparationArea").Result);
        texts.Add("Finishing", Translator.Translate("DressingRoom", "Finishing").Result);
    }

    void Update()
    {
        SetText();
    }

    private void SetText()
    {
        if (G.Instance.Progress.CurrentPackage.activeTasks.Count <= 0)
        {
            currentTextField.text = $"<color=#0be325> {texts["PointsRight"]} \n-----></color>";
            return;
        }

        switch (G.Instance.Progress.CurrentPackage.name)
        {
            case PackageName.EquipmentSelection:
                UpdateText(texts["SelectionOfTools"]);
                return;
            case PackageName.Workspace:
                UpdateText(texts["Workspace"]);
                return;
            case PackageName.CleanUp:
                UpdateText(texts["CleaningTheSpace"]);
                return;
            case PackageName.ChangingRoom:
                UpdateText(texts["DressingRoom"]);
                return;
            case PackageName.PreperationRoom:
                UpdateText(texts["PreparationArea"]);
                return;
            case PackageName.FinishUp:
                UpdateText(texts["Finishing"]);
                return;
        }
    }

    private void UpdateText(string translatedText)
    {
        string text = string.Format("<color=#3f546f> {0} </color> \n{1}", translatedText, UISystem.Instance.Descript);
        currentTextField.text = text;
    }
}
