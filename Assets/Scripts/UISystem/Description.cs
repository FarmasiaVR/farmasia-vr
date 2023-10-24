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
            Translater.Translate("DressingRoom", "PointsRight", (translatedText) =>
            {
                currentTextField.text = $"<color=#0be325> {translatedText} \n-----></color>";
            });
            return;
        }

        switch (G.Instance.Progress.CurrentPackage.name)
        {
            case PackageName.EquipmentSelection:
                Translater.Translate("DressingRoom", "SelectionOfTools", translatedText => UpdateText(translatedText));
                return;
            case PackageName.Workspace:
                Translater.Translate("DressingRoom", "Workspace", translatedText => UpdateText(translatedText));
                return;
            case PackageName.CleanUp:
                Translater.Translate("DressingRoom", "CleaningTheSpace", translatedText => UpdateText(translatedText));
                return;
            case PackageName.ChangingRoom:
                Translater.Translate("DressingRoom", "DressingRoom", translatedText => UpdateText(translatedText));
                return;
            case PackageName.PreperationRoom:
                Translater.Translate("DressingRoom", "PreparationArea", translatedText => UpdateText(translatedText));
                return;
            case PackageName.FinishUp:
                Translater.Translate("DressingRoom", "Finishing", translatedText => UpdateText(translatedText));
                return;
        }
    }

    private void UpdateText(string translatedText)
    {
        string text = string.Format("<color=#3f546f> {0} </color> \n{1}", translatedText, UISystem.Instance.Descript);
        currentTextField.text = text;
    }
}
