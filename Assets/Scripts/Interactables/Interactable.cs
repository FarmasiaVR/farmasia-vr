using UnityEngine;

public class Interactable : MonoBehaviour {

    #region fields
    public EnumBitField<InteractableType> Types { get; protected set; }

    public InteractState State;
    #endregion

    protected virtual void Start() {
        Types = new EnumBitField<InteractableType>();
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