using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;


public class WritingOptions : MonoBehaviour {

    private static System.Random rand = new System.Random();

    public GameObject resultTextObject;
    public GameObject errorTextObject;
    private TextMeshPro resultTextField;
    private TextMeshPro errorTextField;


    public Writable objectToTypeTo;
    private Dictionary<WritingType, string> selectedOptions = new Dictionary<WritingType, string>();
    private WritingType? lastLine = null;
    private string alreadyWrittenText;
    private string resultText;

    // The visibility of the options are controlled with the toggle gameObject
    private GameObject toggle;

    // How many lines can be selected, in addition to the already existing text
    private int maxLines;

    // Ignore ProgressSystem if in tutorial
    public bool inTutorial;

    // Whether the options are initially visible
    public bool visible;

    // Callback that is invoked when the submit button is clicked. The WritingPen will set this.
    public Action<Dictionary<WritingType, string>> onSubmit;

    public Action onCancel = () => { };

    void Start() {
        resultTextField = resultTextObject.GetComponent<TextMeshPro>();
        resultTextField.SetText("");

        errorTextField = errorTextObject.GetComponent<TextMeshPro>();
        errorTextField.SetText("");

        toggle = transform.GetChild(0).gameObject;
        toggle.SetActive(visible);

        onSubmit = (text) => Logger.Warning("WritingOptions submitted with no callback specified");

        SetButtonCallbacks();
    }

    /// <summary>
    /// Finds all option objects below the toggle object, and sets correct callbacks for them.
    /// </summary>
    private void SetButtonCallbacks() {
        // First get the actual options and set the callbacks. true is passed to the method so inactive objects are searched as well.
        WritingOption[] options = toggle.transform.GetComponentsInChildren<WritingOption>(true);
        foreach (WritingOption option in options) {
            option.onSelect = AddOption;
            option.onDeselect = (o) => RemoveOption(o.WritingType);
        }
        // Then the cancel button
        WritingCancel cancel = toggle.transform.GetComponentInChildren<WritingCancel>(true);
        if (cancel == null) Logger.Warning("WritingOptions did not find WritingCancel component!");
        cancel.onSelect = Cancel;

        // Then the submit button
        WritingSubmit submit = toggle.transform.GetComponentInChildren<WritingSubmit>(true);
        if (cancel == null) Logger.Warning("WritingOptions did not find WritingSubmit component!");
        submit.onSelect = Submit;

        // Then the remove button
        WritingRemove remove = toggle.transform.GetComponentInChildren<WritingRemove>(true);
        if (cancel == null) Logger.Warning("WritingOptions did not find WritingRemove component!");
        remove.onSelect = RemoveLine;
    }

    private void AddOption(WritingOption option) {
        
        if (objectToTypeTo)
        {
            objectToTypeTo.addLine(option.WritingType, option.OptionText);
            UpdateResultingText(objectToTypeTo.WrittenLines);
        }
        
       
        UpdateErrorMessage();
    }

    private void RemoveOption(WritingType type) {
       
        if (objectToTypeTo)
        {
            objectToTypeTo.removeLine(type);
            UpdateResultingText(objectToTypeTo.WrittenLines);
        }
      
        UpdateErrorMessage();
    }

    public void UpdateResultingText(Dictionary<WritingType, string> writings) {
        resultText = "";
        foreach (string line in writings.Values) {
            resultText += line + "\n";
        }
        resultTextField.SetText(resultText);

        UpdateErrorMessage();
    }

    private void UpdateErrorMessage() {
        if (objectToTypeTo)
        {
            if(objectToTypeTo.writingsInOrder.Count >= objectToTypeTo.MaxLines) 
            {
                errorTextField.SetText(Translator.Translate("XR MembraneFilteration 2.0", "MaxAmountOfRows"));
            }
            else
            {
                errorTextField.SetText("");
            }
        }
    }

    private void Cancel() {
        onCancel();
        ResetOptions();
    }

    private void Submit() {
        onSubmit(selectedOptions);
        ResetOptions();
    }

    private void RemoveLine() {
      
        if (objectToTypeTo)
        {
            objectToTypeTo.removeLatest();
            UpdateResultingText(objectToTypeTo.WrittenLines);
        }

        if (!lastLine.HasValue) return;
        RemoveOption(lastLine.Value);
    }

    private void ResetOptions() {
        // reset all text stuff. These same fields will be used when the next writing happens
        resultText = "";
        selectedOptions.Clear();
        resultTextField.SetText("");
        errorTextField.SetText("");

        // Toggle all options off
        WritingOption[] options = toggle.transform.GetComponentsInChildren<WritingOption>(true);
        foreach (WritingOption option in options) {
            option.Reset();
            if (option.WritingType == WritingType.FakeTime) {
                option.WritingType = WritingType.Time;
            }
            else if(option.WritingType == WritingType.SecondFakeTime)
            {
                option.WritingType = WritingType.SecondTime;
            }
        }

        SetVisible(false);
    }

    public void SetVisible(bool visible) {
        toggle.SetActive(visible);
    }

    public void SetWritable(Writable writable) {
        objectToTypeTo = writable;
        
        int fakeTimeSet = rand.Next(0, 2);
        alreadyWrittenText = writable.Text;
        // Count how many lines it already has
        int currentLines = alreadyWrittenText.Split('\n').Length - 1; // Why -1? Just trust me.
        maxLines = writable.MaxLines - currentLines;
        
        if (objectToTypeTo)
        {
            UpdateResultingText(objectToTypeTo.WrittenLines);
        }

        UpdateErrorMessage();
        UpdatePosition(writable.transform);
        WritingOption[] options = toggle.transform.GetComponentsInChildren<WritingOption>(true);
        foreach (WritingOption option in options) {
            switch(option.WritingType)
            {
                case WritingType.Time or WritingType.SecondTime:
                    if (!inTutorial && G.Instance.Progress.CurrentPackage.doneTypes.Contains(TaskType.WriteTextsToItems))
                    {
                        option.WritingType = WritingType.SecondTime;
                    }
                    Logger.Print("Writing type: " + option.WritingType);
                    Logger.Print("Writing option: " + option);
                    Logger.Print("Fake time: " + fakeTimeSet);
                    if (fakeTimeSet == 0)
                    {
                        option.WritingType = WritingType.FakeTime;

                        if (!inTutorial && G.Instance.Progress.CurrentPackage.doneTypes.Contains(TaskType.WriteTextsToItems))
                        {
                            option.WritingType = WritingType.SecondFakeTime;
                        }

                        var time = DateTime.UtcNow.ToLocalTime();
                        time = time.AddMinutes(rand.Next(120) - 60);
                        option.UpdateText(time.ToShortTimeString());
                    }
                    else
                    {
                        option.UpdateText(DateTime.UtcNow.ToLocalTime().ToShortTimeString());
                    }
                    fakeTimeSet--;
                    break;
                case WritingType.Date:
                    option.UpdateText(DateTime.UtcNow.ToLocalTime().ToShortDateString());
                    break;
                case WritingType.Name:
                    option.UpdateText(Player.Info.Name ?? Translator.Translate("XR MembraneFilteration 2.0", "Player"));
                    break;
                case WritingType.LeftHand:
                    option.UpdateText(Translator.Translate("XR MembraneFilteration 2.0", "LeftHand"));
                    break;
                case WritingType.RightHand:
                    option.UpdateText(Translator.Translate("XR MembraneFilteration 2.0", "RightHand"));
                    break;
                case WritingType.Tioglygolate:
                    option.UpdateText(Translator.Translate("XR MembraneFilteration 2.0", "Thioglycolate"));
                    break;
                case WritingType.SoyCaseine:
                    option.UpdateText(Translator.Translate("XR MembraneFilteration 2.0", "SoyCaseine"));
                    break;
                case WritingType.Control:
                    var localizedString = new LocalizedString("WritingOptions", "Control");
                    localizedString.StringChanged += (localizedText) => {
                        option.UpdateText(localizedText);
                    };
                    break;
            }
        }
    }

    private void UpdatePosition(Transform writableTransform) {
        // TODO
    }
}
