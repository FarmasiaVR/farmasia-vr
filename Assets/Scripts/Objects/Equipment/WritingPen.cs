using UnityEngine;
using System.Collections;

public class WritingPen : GeneralItem {

    protected override void Start() {
        base.Start();
        objectType = ObjectType.Pen;
        Type.On(InteractableType.Interactable, InteractableType.SmallObject);
    }

    protected override void OnCollisionEnter(Collision other) {
        base.OnCollisionEnter(other);

        // Get the Writable component of the collided item
        GameObject foundObject = GetInteractableObject(other.transform);
        Writable writable = foundObject?.GetComponent<Writable>();
        if (writable == null) {
            return;
        }
        if (!base.IsGrabbed) { // prevent accidental writing when pen not grabbed
            return;
        }

        Write(writable);
    }

    private void Write(Writable writable) {
        
        // Find the writing options object, make it visible, set a callback for when user submits text from the options
        
        var writingGameObject = GameObject.Find("WritingOptions");
        Logger.Print("game object " + writingGameObject);
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

        // Now show it
        writingOptions.SetVisible(true);
        // Give it the writable's info (text and maximum number of lines)
        writingOptions.SetWritable(writable);

        // Set the callback so that it writes to the writable when it is submitted
        writingOptions.onSubmit = (text) => { writable.Text = text; };
    }
}
