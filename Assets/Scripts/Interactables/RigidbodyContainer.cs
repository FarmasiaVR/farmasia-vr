using UnityEngine;

public class RigidbodyContainer {

    private Interactable interactable;
    private RigidbodyState state;

    public Rigidbody Rigidbody { get; private set; }
    public bool Enabled { get; private set; }

    public RigidbodyState State {
        get {
            if (Enabled) {
                throw new System.Exception("Trying to access Rigidbody state while enabled");
            }
            return state;
        }
    }

    public RigidbodyContainer(Interactable interactable) {
        this.interactable = interactable;

        Rigidbody = interactable.GetComponent<Rigidbody>();

        if (Rigidbody == null) {
            throw new System.Exception("Interactable has no rigidbody");
        }

        Enabled = true;
    }

    public void EnableAndDeparent() {

        if (Enabled) {
            return;
        }

        Interactable parentInteractable = Interactable.GetInteractable(interactable.transform.parent);

        if (parentInteractable != null) {
            interactable.transform.parent = parentInteractable.transform.parent;
        }

        Enable();
    }

    public void Enable() {

        if (Rigidbody != null) {
            throw new System.Exception("Rigidbody was not null");
        }

        Enabled = true;

        Rigidbody = interactable.gameObject.AddComponent<Rigidbody>();
        state.SetRigidbody(Rigidbody);
    }
    public void Disable() {

        if (Rigidbody == null) {
            throw new System.Exception("Rigidbody was null");
        }

        Enabled = false;

        SaveState();
        MonoBehaviour.Destroy(Rigidbody);
    }
    public void SaveState() {
        state = new RigidbodyState(Rigidbody);
    }
}