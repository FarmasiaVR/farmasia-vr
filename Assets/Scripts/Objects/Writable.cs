using UnityEngine;
using System.Collections;
using TMPro;

public class Writable : MonoBehaviour {

    // Displays the text, must have a TextMeshPro component
    [SerializeField]
    private GameObject textObject;

    private TextMeshPro textField;

    

    void Start() {
        textField = textObject.GetComponent<TextMeshPro>();
        if (textField == null) {
            Logger.Warning("Writable '" + gameObject.ToString() + "' does not have a valid textObject attached");
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void Write(string text) {
        textField.SetText(text);
    }
}
