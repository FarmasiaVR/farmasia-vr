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

    void Start() {
        if (isInitiallyOn) {
            EnableDisplay();
        } else {
            DisableDisplay();
        }
        // Set the Display script to follow this gameObject.
        Display display = displayObject.GetComponent<Display>();
        Logger.Print("Pipette liquid display following " + gameObject);
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
