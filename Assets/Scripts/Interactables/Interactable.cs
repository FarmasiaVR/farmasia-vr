using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    protected InteractableType type;
    public InteractableType Type { get => type; }

    public InteractState State;

    protected virtual void Start() {
        State = InteractState.None;
        gameObject.AddComponent<ObjectHighlight>();
    }

    public virtual void Interact(Hand hand) {
    }
    public virtual void Interacting(Hand hand) {
    }
    public virtual void Uninteract(Hand hand) {
    }
}