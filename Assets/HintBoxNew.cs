using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintBoxNew : MonoBehaviour
{
    // Add TextMeshPro object in Unity editor
    [SerializeField]
    private TextMeshPro hintDesc;

    private Transform[] hintBoxObjects;

    private float timeSincePress;
    private bool textShown;

    // Start is called before the first frame update
    void Start()
    {
        // Get transforms of children objects
        hintBoxObjects = this.gameObject.GetComponentsInChildren<Transform>();
        // Hide text in the beginning
        hintDesc.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Calls method to rotate the hint box
        RotateHintBox();

        // Checks if text is being shown and after 10 seconds hides it
        if (textShown)
        {
            timeSincePress += Time.deltaTime;
            if (timeSincePress > 10)
            {
                HideText();
            }
        }
    }
    
    /// <summary>
    /// Method that shows the text to the player. Is currently called 
    /// from a XR Simple Interactable's Interactable Events' event.
    /// </summary>
    public void ShowText()
    {
        hintDesc.gameObject.SetActive(true);
        textShown = true;

    }

    /// <summary>
    /// Method to hide the text from the player.
    /// </summary>
    private void HideText()
    {
        hintDesc.gameObject.SetActive(false);
        textShown = false;
    }

    /// <summary>
    /// Method to rotate the question mark and hintbox.
    /// </summary>
    private void RotateHintBox()
    {
        // Index 2 rotates the second child of HintBoxNew which is "Body", 
        // index 3 is the first child of "Body" AKA "HintBoxShape". 
        hintBoxObjects[2].Rotate(Vector3.up * 20 * Time.deltaTime);
        hintBoxObjects[3].Rotate(Vector3.left * 20 * Time.deltaTime);


    }
}
