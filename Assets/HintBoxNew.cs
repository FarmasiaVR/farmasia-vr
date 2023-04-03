using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FarmasiaVR.New;


public class HintBoxNew : MonoBehaviour
{
    // Add TextMeshPro object in Unity editor
    [SerializeField]
    private TextMeshPro hintDesc;

    // Array for child objects of HintBoxNew prefab
    private Transform[] hintBoxObjects;

    // TaskManager is required for Mistake generation
    private TaskManager taskManager;


    [SerializeField]
    private float textShownTime;

    private bool textShown;


    private void Awake()
    {
        // Find TaskManager in the scene
        taskManager = FindObjectOfType<TaskManager>();
    }
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
    public void HideText()
    {
        // Checks if text is being shown and after given time in seconds hides it.
        // The time is set in Unity editor.
        if (textShown)
        {
            //timeSincePress -= Time.deltaTime;
            //Debug.Log("timeSincePress value: " + timeSincePress + " and deltaTime value: " + Time.deltaTime);
            TextTimer(textShownTime);
            Debug.Log("Returned from timer");
            hintDesc.gameObject.SetActive(false);
            textShown = false;
        }

    }

    private IEnumerator TextTimer(float showTextTime)
    {
        yield return new WaitForSeconds(showTextTime);
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

    /// <summary>
    /// Generates a Mistake into the TaskList as well as a Popup with the
    /// error message through TaskManager. 
    /// </summary>
    public void TextShownMistake()
    {
        taskManager.GenerateGeneralMistake("Vinkkilaatikko avattu!", 2);
    }

    /// <summary>
    /// Boolean value that tells if the text is showing or not. No use currently.
    /// </summary>
    /// <returns></returns>
    public bool GetTextShown()
    {
        return textShown;
    }
}
