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
        Enabled = true;
    }

    public void EnableAndDeparent() {

        if (Enabled) {
            Logger.Warning("Trying to add rigidbody again");
            return;
        }

        Interactable parentInteractable = Interactable.GetInteractable(interactable.transform.parent);

        if (parentInteractable != null) {
            interactable.transform.parent = parentInteractable.transform.parent;
        }

        Enable();
    }

    public void Enable() {

        if (Enabled) {
            Logger.Warning("Trying to add rigidbody again");
            return;
        }

        if (Rigidbody != null) {
            throw new System.Exception("Rigidbody was not null");
        }

        if (interactable.GetComponent<Rigidbody>() != null) {
            throw new System.Exception("Rigidbody via GetComponent was not null");
        }

        Enabled = true;

        Rigidbody = interactable.gameObject.AddComponent<Rigidbody>();

        if (Rigidbody == null) {
            Logger.Error("Added rigidbody was niull");
        }

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