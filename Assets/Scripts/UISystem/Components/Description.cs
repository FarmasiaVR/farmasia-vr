using UnityEngine;
using TMPro;

public class Description : MonoBehaviour {
    #region Fields
    private GameObject textObject;
    private TextMeshPro textField;
    private float transVisible = 1.0f;
    private float transHidden = 0.5f;
    #endregion

    private void Awake() {
        textObject = transform.gameObject;
        textField = textObject.GetComponent<TextMeshPro>();
    }

    private void SetColor(Color color) {
        textField.color = new Color(color.r, color.g, color.b, transVisible);
    }

    public void SetTransparency(bool visible) {
        if (visible) {
            textField.color = new Color(textField.color.r, textField.color.g, textField.color.b, transVisible);
            return;
        }
        textField.color = new Color(textField.color.r, textField.color.g, textField.color.b, transHidden);
    }

    public void SetDescription(string description, Color color) {
        textField.text = description;
        SetColor(color);
    }

}
