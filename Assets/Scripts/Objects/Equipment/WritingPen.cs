using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WritingPen : GeneralItem {

    private bool isWriting;
    public bool ignoreGrabCheck;
    protected override void Start() {
        base.Start();
        objectType = ObjectType.Pen;
        Type.On(InteractableType.Interactable);
    }

    protected override void OnCollisionEnter(Collision other) {
        base.OnCollisionEnter(other);

        // Get the Writable component of the collided item
        GameObject foundObject = GetInteractableObject(other.transform);
        Writable writable = foundObject?.GetComponent<WritingTarget>()?.GetWritable();
        if (writable == null) {
            return;
        }
        Debug.Log("Detected writable!");
        if (ignoreGrabCheck != true) {
            if (!base.IsGrabbed) { // prevent accidental writing when pen not grabbed
                return;
            }
        }

        if (isWriting) { // must submit or cancel before selecting another item
            return;
        }
        Debug.Log("called pen Write function");
        Write(writable, foundObject);
    }

    private void Write(Writable writable, GameObject foundObject) {
        
        // Find the writing options object, make it visible, set a callback for when user submits text from the options
        
        var writingGameObject = GameObject.Find("WritingOptions");
        if (writingGameObject == null) {
            Logger.Print("Writing options not found");
            return;
        }
        Logger.Print("Opened writing options");
        var writingOptions = writingGameObject.GetComponent<WritingOptions>();
        if (writingOptions == null) {
            Logger.Print("Did not found gameObject writing options");
            return;
        }
        isWriting = true;

        // Now show it
        writingOptions.SetVisible(true);

       
        // Give it the writable's info (text and maximum number of lines)
        writingOptions.SetWritable(writable);


        
        
        // Set the callback so that it writes to the writable when it is submitted
        writingOptions.onSubmit = (selectedOptions) => {
            SubmitWriting(writable, foundObject, selectedOptions);
            isWriting = false;
            writingOptions.objectToTypeTo = null;
        };
        writingOptions.onCancel = () => {
            isWriting = false;
            writingOptions.objectToTypeTo = null;
        };
    }

    public void SubmitWriting(Writable writable, GameObject foundObject, Dictionary<WritingType, string> selectedOptions) {
        writable.AddWrittenLines(selectedOptions);
        Events.FireEvent(EventType.WriteToObject, CallbackData.Object(foundObject));
    }
}
