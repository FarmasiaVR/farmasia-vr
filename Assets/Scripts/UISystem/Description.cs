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
        string text = "<color=#000000> " + G.Instance.Progress.CurrentPackage.name  + " </color> \n";
        text += UISystem.Instance.Descript;
        currentTextField.text = text;
    }
}
