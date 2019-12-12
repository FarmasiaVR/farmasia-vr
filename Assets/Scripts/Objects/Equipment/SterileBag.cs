using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SterileBag : GeneralItem {

    #region fields

    public List<Syringe> Syringes { get; private set; }

    public List<GameObject> objectsInBag;
    public bool IsClosed { get; private set; }
    public bool IsSterile { get; private set; }
    [SerializeField]
    private GameObject childCollider;
    #endregion

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        Syringes = new List<Syringe>();

        ObjectType = ObjectType.SterileBag;

        objectsInBag = new List<GameObject>();
        IsClosed = false;
        IsSterile = true;

        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => OnBagEnter(collider)));
    }

    private void OnBagEnter(Collider other) {

        Logger.Print("OnBagEnter: " + other.name);

        Syringe syringe = other.GetComponent<Syringe>();

        if (syringe == null) {
            return;
        }

        if (syringe.IsAttached) {
            return;
        }

        if (Syringes.Count >= 6) {
            return;
        }

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

        foreach (Syringe s in Syringes) {
            ReleaseSyringe(s);
        }
        Syringes.Clear();
        IsClosed = false;
    }

    private void SetSyringe(Syringe syringe) {
        syringe.RigidbodyContainer.Disable();
        syringe.GetComponent<Collider>().enabled = false;

        syringe.transform.SetParent(transform);

        syringe.transform.localPosition = ObjectPosition(Syringes.Count);
        syringe.transform.localEulerAngles = new Vector3(180, 180, 0);
        Syringes.Add(syringe);
    }
    private void ReleaseSyringe(Syringe syringe) {
        syringe.GetComponent<Collider>().enabled = true;
        syringe.RigidbodyContainer.EnableAndDeparent();
    }

    private void CloseSterileBag() {
        IsClosed = true;
        Events.FireEvent(EventType.SterileBag, CallbackData.Object(this));
    }

    private Vector3 ObjectPosition(int index) {

        Vector3 pos = new Vector3(0, 0.172f, -0.017f);
        pos.x = (0.2f / 5) * index - 0.1f;

        return pos;
    }
}