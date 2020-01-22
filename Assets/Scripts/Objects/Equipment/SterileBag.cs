using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SterileBag : GeneralItem {

    #region fields

    public List<Syringe> Syringes { get; private set; }
    public bool IsClosed { get; private set; }
    public bool IsSterile { get; private set; }
    [SerializeField]
    private GameObject childCollider;

    private float ejectSpeed = 0.5f;

    private bool timeout;

    private float timeoutTime = 0.5f;
    #endregion

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        Syringes = new List<Syringe>();

        ObjectType = ObjectType.SterileBag;

        IsClosed = false;
        IsSterile = true;

        Type.On(InteractableType.Interactable);

        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => OnBagEnter(collider)));
    }

    private void OnBagEnter(Collider other) {

        if (timeout) {
            return;
        }

        Syringe syringe = Interactable.GetInteractable(other.transform) as Syringe;

        if (syringe == null) {
            return;
        }

        if (syringe.IsAttached) {
            return;
        }

        if (Syringes.Count >= 6) {
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

        Events.FireEvent(EventType.SterileBag, CallbackData.Object(this));

        if (Syringes.Count == 6) {
            CloseSterileBag();
        }
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);

        if (timeout) {
            return;
        }

        float angle = Vector3.Angle(Vector3.down, transform.up);

        if (angle < 45) {
            return;
        }

        timeout = true;
        G.Instance.Pipeline
            .New()
            .Delay(timeoutTime)
            .Func(() => timeout = false);

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
        
        // syringe.transform.localPosition = syringe.transform.localPosition + new Vector3(0, 0, 0.05f);
        syringe.transform.SetParent(null);

        // CollisionIgnore.IgnoreCollisions(transform, syringe.transform, true);

        //G.Instance.Pipeline
        //    .New()
        //    .Delay(timeoutTime)
        //    .Func(() => {
        //        SetColliders(syringe.transform, true);
        //       // CollisionIgnore.IgnoreCollisions(transform, syringe.transform, false);
        //        syringe.RigidbodyContainer.Enable();
        //    });

        StartCoroutine(MoveSyringe(syringe, transform.up));
    }

    private IEnumerator MoveSyringe(Syringe syringe, Vector3 dir) {

        float time = 0;

        float distance = 0;

        while (time < timeoutTime) {
            time += Time.deltaTime;
            distance += Time.deltaTime * ejectSpeed;
            syringe.transform.position = transform.position + transform.up * distance;
            yield return null;
        }

        SetColliders(syringe.transform, true);
        syringe.RigidbodyContainer.Enable();
    }

    private void CloseSterileBag() {
        IsClosed = true;
        Events.FireEvent(EventType.SterileBag, CallbackData.Object(this));
    }

    private Vector3 ObjectPosition(int index) {

        Vector3 pos = new Vector3(0, 0.172f, 0);
        pos.x = (0.2f / 5) * index - 0.1f;

        return pos;
    }
}