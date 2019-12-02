using TMPro;
using UnityEngine;

public class DescriptionDuplicate : MonoBehaviour {
    #region Fields
    private GameObject textObject;
    private TextMeshPro textField;
    private TextMeshPro currentTextField;
    #endregion

    // Start is called before the first frame update
    void Start() {
        currentTextField = gameObject.GetComponent<TextMeshPro>();
        FindDescObject();
    }

    // Update is called once per frame
    void Update() {
        if (textObject == null || textField == null) {
            FindDescObject();
            return;
        }
        SetText();
    }

    private void SetText() {
        string text = "<color=#000000> " + G.Instance.Progress.CurrentPackage.name  + " </color> \n";
        text += textField.text;
        currentTextField.text = text;
    }

    private void FindDescObject() {
        textObject = GameObject.FindGameObjectWithTag("Description");
        textField = textObject.GetComponent<TextMeshPro>();
    }
}
