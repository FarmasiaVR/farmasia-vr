using TMPro;
using UnityEngine;

public class Description : MonoBehaviour {
    #region Fields
    private TextMeshPro currentTextField;
    #endregion

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
                packageName = "Työskentely Tila";
                break;
            case PackageName.CleanUp:
                packageName = "Tilan Siivoaminen";
                break;
        }
        


        string text = "<color=#000000> " + packageName  + " </color> \n";
        text += UISystem.Instance.Descript;
        currentTextField.text = text;
    }
}
