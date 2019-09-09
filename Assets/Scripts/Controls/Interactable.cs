using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    [SerializeField]
    private GrabType type;
    public GrabType Type { get => type; }

    public abstract void Interact();
}