using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glasses : Grabbable {

    private bool insideTapCollider;
    private bool insidePlayerCollider;
    private bool isClean;

    public HandWashingLiquid sink;
    public GameObject tapCollider;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);
    }

    public override void OnGrab(Hand hand) {
        if (!isClean) {
            if (!insideTapCollider || !sink.IsRunning()) return;
            isClean = true;
            Events.FireEvent(EventType.CleaningGlasses, CallbackData.Object(this));
        }
        if (insidePlayerCollider) {
            insidePlayerCollider = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.transform == tapCollider.transform) insideTapCollider = true;
        if (other.CompareTag("PlayerCollider")) insidePlayerCollider = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.transform == tapCollider.transform) insideTapCollider = false;
        if (other.CompareTag("PlayerCollider")) insidePlayerCollider = false;
    }
}
