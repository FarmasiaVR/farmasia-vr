using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SterileBag2 : GeneralItem {

    #region fields
    public SyringeNew syringe1;

    public SyringeCapConnect cap1;

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

        if (syringe1 != null && cap1 != null) SetSyringe(syringe1, cap1);

        ObjectType = ObjectType.SterileBag;

        IsClosed = false;
        IsSterile = true;

        Type.On(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);

        if (finalClose) {
            return;
        }

        float angle = Vector3.Angle(Vector3.down, transform.up);

        if (angle < 45) {
            return;
        }

        Logger.Print("Release syringes");
        
        ReleaseSyringe();
        IsClosed = false;
    }

    private void SetSyringe(SyringeNew syringe, SyringeCapConnect cap) {

        syringe.RigidbodyContainer.Disable();
        cap.RigidbodyContainer.Disable();
        SetColliders(syringe.transform, false);
        SetColliders(cap.transform, false);

        syringe.transform.SetParent(transform);
        cap.transform.SetParent(transform);

        syringe.transform.localPosition = ObjectPosition(1, false);
        cap.transform.localPosition = ObjectPosition(1, true);
        syringe.transform.localEulerAngles = new Vector3(180, 180, 0);
        cap.transform.localEulerAngles = new Vector3(180, 180, 0);
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

    public void ReleaseSyringe() {
        StartCoroutine(MoveSyringe());
    }

    private IEnumerator MoveSyringe() {
        float totalDistance = 0;

        while (totalDistance < ejectDistance) {
            float distance = Time.deltaTime * ejectSpeed;
            totalDistance += distance;
            syringe1.transform.localPosition += Vector3.up * distance;
            cap1.transform.localPosition += Vector3.up * distance;
            yield return null;
        }

        syringe1.transform.SetParent(null);
        cap1.transform.SetParent(null);
        SetColliders(syringe1.transform, true);
        SetColliders(cap1.transform, true);
        syringe1.RigidbodyContainer.Enable();
        cap1.RigidbodyContainer.Enable();

        syringe1.CheckDistanceAndConnect();
    }

    private Vector3 ObjectPosition(int index, bool isCap) {

        Vector3 pos = new Vector3(0, 0.172f, 0);
        pos.x = (0.2f / 5) * index - 0.1f;

        if (isCap) {
            pos.y = 0.06f;
        }
        return pos;
    }
}