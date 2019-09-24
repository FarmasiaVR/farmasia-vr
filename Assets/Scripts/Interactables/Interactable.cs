using UnityEngine;

public class Interactable : MonoBehaviour {

    #region fields
    protected int typeFlags;
    // protected InteractableType type;
    // public int TypeFlags { get => typeFlags; }
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

    public bool GetFlags(params InteractableType[] types) {
        return BitField.GetFlags(typeFlags, types);
    }
    public void SetFlags(bool value, params InteractableType[] types) {
        BitField.SetFlags(ref typeFlags, value, types);
    }
}