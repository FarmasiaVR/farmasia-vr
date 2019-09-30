using UnityEngine;

public class Interactable : MonoBehaviour {

    #region fields
    private static string iTag = "Interactable";

    public EnumBitField<InteractableType> Types { get; protected set; } = new EnumBitField<InteractableType>();

    public InteractState State;
    #endregion

    protected virtual void Start() {
        Types = new EnumBitField<InteractableType>();
        State = InteractState.None;
        gameObject.AddComponent<ObjectHighlight>();
        gameObject.AddComponent<ItemPlacement>();

        gameObject.tag = iTag;
    }

    public virtual void Interact(Hand hand) {
    }
    public virtual void Interacting(Hand hand) {
    }
    public virtual void Uninteract(Hand hand) {
    }

    public static Interactable GetInteractable(Transform t) {
        return GetInteractableObject(t)?.GetComponent<Interactable>();
    }
    public static GameObject GetInteractableObject(Transform t) {

        while (t != null) {
            if (t.tag == iTag) {
                return t.gameObject;
            }

            t = t.parent;
        }

        return null;
    }
}