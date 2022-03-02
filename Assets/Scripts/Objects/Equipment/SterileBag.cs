using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SterileBag : GeneralItem {

    #region fields
    public Syringe syringe1;
    public Syringe syringe2;
    public Syringe syringe3;
    public Syringe syringe4;
    public Syringe syringe5;
    public Syringe syringe6;

    public List<Syringe> Syringes { get; private set; }
    public bool IsClosed { get; private set; }
    public bool IsSterile { get; private set; }
    [SerializeField]
    private GameObject childCollider;

    [SerializeField]
    private DragAcceptable closeButton;

    private float ejectSpeed = 0.6f;
    private float ejectDistance = 0.47f;
    private bool finalClose;
    #endregion

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        Syringes = new List<Syringe>();
        if (syringe1 != null) SetSyringe(syringe1);
        if (syringe2 != null) SetSyringe(syringe2);
        if (syringe3 != null) SetSyringe(syringe3);
        if (syringe4 != null) SetSyringe(syringe4);
        if (syringe5 != null) SetSyringe(syringe5);
        if (syringe6 != null) SetSyringe(syringe6);

        ObjectType = ObjectType.SterileBag;

        IsClosed = false;
        IsSterile = true;

        Type.On(InteractableType.Interactable);

        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => OnBagEnter(collider)));

        if (closeButton != null) {
            Logger.Print("Initializing bag");
            closeButton.ActivateCountLimit = 1;
            closeButton.OnAccept = CloseSterileBagFinal;
            closeButton.Hide(true);
        }
    }

    private void OnBagEnter(Collider other) {

        Syringe syringe = Interactable.GetInteractable(other.transform) as Syringe;

        if (syringe == null) {
            return;
        }

        if (syringe.IsAttached) {
            return;
        }

        if (Syringes.Count == 6) {
            return;
        }

        if (syringe.State == InteractState.Grabbed) {
            Hand.GrabbingHand(syringe).Connector.Connection.Remove();
        }

        VRInput.Hands[0].Hand.HandCollider.RemoveInteractable(syringe);
        VRInput.Hands[0].Hand.ExtendedHandCollider.RemoveInteractable(syringe);
        VRInput.Hands[1].Hand.HandCollider.RemoveInteractable(syringe);
        VRInput.Hands[1].Hand.ExtendedHandCollider.RemoveInteractable(syringe);

        Logger.Print("Set syringe");

        SetSyringe(syringe);

        if (syringe.IsClean) {
            IsSterile = false;
        }

        if (Syringes.Count == 6) {
            EnableClosing();
        }
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);

        if (finalClose) {
            return;
        }

        DisableClosing();

        float angle = Vector3.Angle(Vector3.down, transform.up);

        if (angle < 45) {
            return;
        }

        Logger.Print("Release syringes");

        foreach (Syringe s in Syringes) {
            ReleaseSyringe(s);
        }
        Syringes.Clear();
        IsClosed = false;
    }

    private void SetSyringe(Syringe syringe) {

        syringe.RigidbodyContainer.Disable();
        SetColliders(syringe.transform, false);

        syringe.transform.SetParent(transform);

        syringe.transform.localPosition = ObjectPosition(Syringes.Count);
        syringe.transform.localEulerAngles = new Vector3(180, 180, 0);
        Syringes.Add(syringe);
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

        System.Console.WriteLine("Opening sterilebag");

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

        IsClosed = true;
        closeButton.Hide(true);
    }

    public void CloseSterileBagFinal() {
        finalClose = true;
        closeButton.SafeDestroy();
        Logger.Print("Close Sterile bag Final!");
        Events.FireEvent(EventType.CloseSterileBag, CallbackData.Object(this));
        CleanupObject.GetCleanup().EnableCleanup();
    }

    private Vector3 ObjectPosition(int index) {

        Vector3 pos = new Vector3(0, 0.172f, 0);
        pos.x = (0.2f / 5) * index - 0.1f;

        return pos;
    }
}