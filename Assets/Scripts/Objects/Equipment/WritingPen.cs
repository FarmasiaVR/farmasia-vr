using UnityEngine;
using System.Collections;

public class WritingPen : GeneralItem {

    protected override void Start() {
        base.Start();

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
    }
}
