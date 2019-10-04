using UnityEngine;

public class GrabFunctionality : MonoBehaviour {

    public virtual void Interact(Interactable interactable) {
        Logger.Warning("Interact Not implemented");
    }
    public virtual void Uninteract(Interactable interactable) {
        Logger.Warning("Uninteract Not implemented");
    }

    public virtual void GrabInteract(Interactable interactable) {
        Logger.Warning("GrabInteract Not implemented");
    }
    public virtual void GrabUninteract(Interactable interactable) {
        Logger.Warning("GrabUninteract Not implemented");
    }

    public static RigidbodyGrab AddRigidbodyGrab(GameObject gameObject) {
        return gameObject.AddComponent<RigidbodyGrab>();
    }
}