using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    protected GrabType type;
    public GrabType Type { get => type; }

    protected virtual void Start() {
        gameObject.AddComponent<ObjectHighlight>();
    }

    public virtual void Interact() {

    }
}