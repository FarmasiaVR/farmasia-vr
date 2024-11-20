using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FarmasiaVR.New;


public class HintBoxNew : MonoBehaviour
{
    // Add TextMeshPro object in Unity editor
    [field:SerializeField]
    public TextMeshPro hintDesc { get; private set; }

    // Array for child objects of HintBoxNew prefab
    private Transform[] hintBoxObjects;

    // TaskManager is required for Mistake generation
    private TaskManager taskManager;

    // Given time for the text to show
    [SerializeField]
    private float textShownTime;

    // Value used to reset the textShownTime timer value
    private float textResetValue;
    
    // Boolean used to check if text has been shown to the player
    private bool textShown;


    private void Awake()
    {
        // Find TaskManager in the scene
        taskManager = FindObjectOfType<TaskManager>();
        // Set reset value
        textResetValue = textShownTime;
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

        // Checks if text has been shown
        if (textShown)
        {
            // Decrements until less than zero then calls method to hide the text
            textShownTime -= Time.deltaTime;            
            if (textShownTime < 0)
            {
                HideText();
            }
        }
    }

    /// <summary>
    /// Method that shows the text to the player. Recommended to be used with an XR
    /// Interactable.
    /// </summary>
    public void ShowText()
    {
        hintDesc.gameObject.SetActive(true);
        textShown = true;
        textShownTime = textResetValue;
    }

    /// <summary>
    /// Method to hide the text from the player.
    /// </summary>
    public void HideText()
    {
        // Checks if text is being shown and after given time in seconds hides it.
        // The time is set in Unity editor.
        hintDesc.gameObject.SetActive(false);
        textShown = false;
    }
    /// <summary>
    /// Updates the text that the hint box shows
    /// </summary>
    /// <param name="newText">The text to be shown to the player</param>
    public void UpdateText(string newText)
    {
        hintDesc.text= newText;
    }

    public void UpdateText(Task task)
    {
        // hintDesc.text = task.hint;
        string hintTextLocal = task.key + "Hint";
        string translatedText = Translator.Translate("ControlsTutorial", hintTextLocal);
        UpdateText(translatedText);
        ShowText();
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
    /// error message through TaskManager. Recommended to be used with an XR Interactable.
    /// </summary>
    public void TextShownMistake()
    {
        taskManager.GenerateGeneralMistake("Vinkkilaatikko avattu!", 2);
    }

    /// <summary>
    /// Boolean value that tells if the text is showing or not.
    /// </summary>
    /// <returns></returns>
    public bool GetTextShown()
    {
        return textShown;
    }
}
