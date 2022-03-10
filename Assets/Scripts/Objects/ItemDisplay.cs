using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// Controls the Display for this gameObject.
/// </summary>
public class ItemDisplay : MonoBehaviour {

    [SerializeField]
    private GameObject displayObject;

    [SerializeField]
    private bool isInitiallyOn = false;

    private bool isDisplayOn = false;

    private TextMeshPro text;

    public string Text
    {
        get { return text.GetParsedText(); }
    }

    void Start() {
        TextMeshPro text = gameObject.GetComponent(typeof(TextMeshPro)) as TextMeshPro;
        if (text == null)
        {
            Logger.Warning("Writable '" + gameObject.ToString() + "' does not have a valid textObject attached");
        }

        if (isInitiallyOn) {
            EnableDisplay();
        } else {
            DisableDisplay();
        }
        // Set the Display script to follow this gameObject.
        Display display = displayObject.GetComponent<Display>();
        display.SetFollowedObject(gameObject);
    }

    public void EnableDisplay() {
        if (isDisplayOn) {
            return;
        }
        isDisplayOn = true;
        // Make the display visible
        displayObject.SetActive(true);
    }

    public void DisableDisplay() {
        displayObject.SetActive(false);
        isDisplayOn = false;
    }
}
