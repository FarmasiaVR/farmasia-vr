using UnityEngine;

public class Interactable : MonoBehaviour {

    #region fields
    protected InteractableType type;
    public InteractableType Type { get => type; }
    #endregion

    public InteractState State;

    protected virtual void Start() {
        State = InteractState.None;
        gameObject.AddComponent<ObjectHighlight>();
        gameObject.AddComponent<ItemPlacement>();
    }

    public virtual void Interact(Hand hand) {
    }
    public virtual void Interacting(Hand hand) {
    }
    public virtual void Uninteract(Hand hand) {
    }
}