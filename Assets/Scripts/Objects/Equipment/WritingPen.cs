using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WritingPen : GeneralItem {

    private bool isWriting;
    public bool ignoreGrabCheck;
    public GameObject collidedObject;
    public bool writeOnTouch;
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
        
        if (writeOnTouch)
        {
            Write(writable, foundObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        GameObject foundObject = GetInteractableObject(other.transform);
        Writable writable = foundObject?.GetComponent<WritingTarget>()?.GetWritable();
        if (writable == null)
        {
            return;
        }
        Debug.Log("Detected writable!");
        

        if (isWriting)
        { // must submit or cancel before selecting another item
            return;
        }
        if (collidedObject == null) {
            collidedObject = foundObject;
        }
        
            
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<WritingTarget>())
        {
            collidedObject = null;
        }
    }

    public void WriteXR()
    {
        if (collidedObject != null)
        {
            Writable write = collidedObject.GetComponent<Writable>();
            if (write != null)
            {
                Write(write, collidedObject);
            }
        }
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
            collidedObject = null;
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
