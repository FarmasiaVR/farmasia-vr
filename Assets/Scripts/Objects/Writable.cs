using UnityEngine;
using System.Collections;
using TMPro;

public class Writable : MonoBehaviour {

    // Displays the text, must have a TextMeshPro component
    [SerializeField]
    private GameObject textObject;

    [SerializeField]
    public int MaxLines = 4;

    private TextMeshPro textField;

    public string Text {
        get { return textField.GetParsedText(); }
        set { textField.SetText(value); }
    }
    
    void Start() {
        textField = textObject.GetComponent<TextMeshPro>();
        if (textField == null) {
            Logger.Warning("Writable '" + gameObject.ToString() + "' does not have a valid textObject attached");
        }
    }
}
