using UnityEngine;
using System.Collections;

public class WritingPen : GeneralItem {

    // Use this for initialization
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

        Logger.Print("Writing stuff!");
    }
}
