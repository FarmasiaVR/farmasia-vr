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

        Write(writable);
    }

    private void Write(Writable writable) {
        bool didWrite = writable.Write("Kirjoitusta");
        if (didWrite) {
            Logger.Print("Wrote stuff with pen!");
        }
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
        writingOptions.SetVisible(true);
        
    }
}
