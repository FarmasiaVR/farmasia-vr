using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SterileBag : GeneralItem {

    private float ejectSpeed = 0.6f;
    private float ejectDistance = 0.47f;
    private bool finalClose;

    public List<Syringe> syringes;
    public GameObject childCollider;
    public DragAcceptable closeButton;
    public bool isClosed;

    protected override void Start() {
        base.Start();
        syringes = new List<Syringe>();
        Type.On(InteractableType.Interactable);
        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => OnBagEnter(collider)));
        if (closeButton != null) {
            closeButton.ActivateCountLimit = 1;
            closeButton.OnAccept = CloseSterileBagFinal;
            closeButton.Hide(true);
        }
    }

    private void OnBagEnter(Collider other) {
        Syringe syringe = GetInteractable(other.transform) as Syringe;
        if (syringe == null) {
            return;
        }
        if (syringe.Container.Capacity != 1000) {
            return;
        }
        if (syringe.IsAttached) {
            return;
        }
        if (syringes.Count == 6) {
            return;
        }
        if (syringe.State == InteractState.Grabbed) {
            Hand.GrabbingHand(syringe).Connector.Connection.Remove();
        }
        VRInput.Hands[0].Hand.HandCollider.RemoveInteractable(syringe);
        VRInput.Hands[0].Hand.ExtendedHandCollider.RemoveInteractable(syringe);
        VRInput.Hands[1].Hand.HandCollider.RemoveInteractable(syringe);
        VRInput.Hands[1].Hand.ExtendedHandCollider.RemoveInteractable(syringe);
        SetSyringe(syringe);
        if (syringes.Count == 6) {
            EnableClosing();
        }
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        if (finalClose) {
            return;
        }
        DisableClosing();
        foreach (Syringe s in syringes) {
            ReleaseSyringe(s);
        }
        syringes.Clear();
        isClosed = false;
    }

    private void SetSyringe(Syringe syringe) {
        syringe.RigidbodyContainer.Disable();
        SetColliders(syringe.transform, false);
        syringe.transform.SetParent(transform);
        syringe.transform.localPosition = ObjectPosition(syringes.Count);
        syringe.transform.localEulerAngles = new Vector3(180, 180, 0);
        syringes.Add(syringe);
    }

    private void SetColliders(Transform t, bool enabled) {
        Collider coll = t.GetComponent<Collider>();
        if (coll != null) {
            coll.enabled = enabled;
        }
        foreach (Transform child in t) {
            SetColliders(child, enabled);
        }
    }

    private void ReleaseSyringe(Syringe syringe) {
        StartCoroutine(MoveSyringe(syringe));
    }

    private IEnumerator MoveSyringe(Syringe syringe) {
        float totalDistance = 0;
        while (totalDistance < ejectDistance) {
            float distance = Time.deltaTime * ejectSpeed;
            totalDistance += distance;
            syringe.transform.localPosition += Vector3.up * distance;
            yield return null;
        }
        syringe.transform.SetParent(null);
        SetColliders(syringe.transform, true);
        syringe.RigidbodyContainer.Enable();
    }

    private void EnableClosing() {
        if (closeButton == null) {
            return;
        }
        if (closeButton.IsGrabbed) {
            Hand.GrabbingHand(closeButton).Uninteract();
        }
        closeButton.Hide(false);
        closeButton.gameObject.SetActive(true);
    }

    private void DisableClosing() {
        if (closeButton == null) {
            return;
        }
        isClosed = true;
        closeButton.Hide(true);
    }

    public void CloseSterileBagFinal() {
        finalClose = true;
        closeButton.SafeDestroy();
        Events.FireEvent(EventType.CloseSterileBag, CallbackData.Object(this));
        CleanUpObject.GetCleanUpObject().EnableCleanUp();
    }

    private Vector3 ObjectPosition(int index) {
        Vector3 pos = new Vector3(0.0f, 0.172f, 0.0f) {
            x = (0.2f / 5) * index - 0.1f
        };
        return pos;
    }
}
