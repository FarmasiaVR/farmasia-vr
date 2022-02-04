using UnityEngine;
using System.Collections;
using TMPro;

public class Writable : MonoBehaviour {

    // Displays the text, must have a TextMeshPro component
    [SerializeField]
    private GameObject textObject;

    private TextMeshPro textField;

    // Tracks if something has been written. If true, cannot write again. 
    [SerializeField]
    private bool isWritten;
    
    void Start() {
        textField = textObject.GetComponent<TextMeshPro>();
        if (textField == null) {
            Logger.Warning("Writable '" + gameObject.ToString() + "' does not have a valid textObject attached");
        }
    }

    /// <summary>
    /// Sets the text of this writable if not written previously. 
    /// </summary>
    /// <param name="text"></param>
    /// <returns>Whether true if writing succeeded and false if the writable already had writing</returns>
    public bool Write(string text) {
        if (isWritten) return false;
        textField.SetText(text);
        isWritten = true;
        return true;
    }
}
