using TMPro;
using UnityEngine;

public class Description : MonoBehaviour
{
    private TextMeshPro currentTextField;

    void Start()
    {
        currentTextField = gameObject.GetComponent<TextMeshPro>();
    }

    void Update()
    {
        SetText();
    }

    private void SetText()
    {
        if (G.Instance.Progress.CurrentPackage.activeTasks.Count <= 0)
        {
            currentTextField.text = $"<color=#0be325> {Translator.Translate("DressingRoom", "PointsRight")} \n-----></color>";
            return;
        }

        switch (G.Instance.Progress.CurrentPackage.name)
        {
            case PackageName.EquipmentSelection:
                UpdateText(Translator.Translate("DressingRoom", "SelectionOfTools"));
                return;
            case PackageName.Workspace:
                UpdateText(Translator.Translate("DressingRoom", "Workspace"));
                return;
            case PackageName.CleanUp:
                UpdateText(Translator.Translate("DressingRoom", "CleaningTheSpace"));
                return;
            case PackageName.ChangingRoom:
                UpdateText(Translator.Translate("DressingRoom", "DressingRoom"));
                return;
            case PackageName.PreperationRoom:
                UpdateText(Translator.Translate("DressingRoom", "PreparationArea"));
                return;
            case PackageName.FinishUp:
                UpdateText(Translator.Translate("DressingRoom", "Finishing"));
                return;
        }
    }

    private void UpdateText(string translatedText)
    {
        string text = string.Format("<color=#3f546f> {0} </color> \n{1}", translatedText, UISystem.Instance.Descript);
        currentTextField.text = text;
    }
}
