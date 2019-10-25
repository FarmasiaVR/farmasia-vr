using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMeshToggler : MonoBehaviour {

    private Renderer[] renderers;
    private Hand hand;
    private bool status;

    void Start() {
        hand = GetComponent<Hand>();
        status = enabled;
        renderers = GetComponentsInChildren<Renderer>();

        Events.SubscribeToEvent(UpdateMesh, this, EventType.InteractWithObject);
        Events.SubscribeToEvent(UpdateMesh, this, EventType.UninteractWithObject);
        Events.SubscribeToEvent(UpdateMesh, this, EventType.GrabInteractWithObject);
        Events.SubscribeToEvent(UpdateMesh, this, EventType.GrabUninteractWithObject);
    }

    private void UpdateMesh(CallbackData data) {

        Hand hand = data.DataObject as Hand;

        if (hand.HandType != this.hand.HandType) {
            return;
        }

        if (status != hand.IsGrabbed) {
            status = hand.IsGrabbed;
            SetRenderers();
        }
    }


    private void SetRenderers() {
        foreach (Renderer r in renderers) {
            r.enabled = status;
        }
    }
}
