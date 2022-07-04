using TMPro;
using UnityEngine;

public class Description : MonoBehaviour {

    private TextMeshPro currentTextField;

    void Start() {
        currentTextField = gameObject.GetComponent<TextMeshPro>();
    }

    void Update() {
        SetText();
    }

    private void SetText() {
        string packageName = "";
        if (G.Instance.Progress.CurrentPackage.activeTasks.Count <= 0) {
            currentTextField.text = "<color=#0be325> Pisteet oikealla \n-----></color>";
            return;
        }

        switch (G.Instance.Progress.CurrentPackage.name) {
            case PackageName.EquipmentSelection:
                packageName = "Työvälineiden valinta";
                break;
            case PackageName.Workspace:
                packageName = "Työskentely tila";
                break;
            case PackageName.CleanUp:
                packageName = "Tilan siivoaminen";
                break;
        }

        string text = string.Format("<color=#000000> {0} </color> \n{1}", packageName, UISystem.Instance.Descript);
        currentTextField.text = text;
    }
}
