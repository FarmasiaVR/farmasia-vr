using UnityEngine;
using System.Collections;

public class BottleCapConnector : AttachmentConnector {
    public override ItemConnection Connection { get; set; }

    public BottleCapConnector(BottleCap needle, GameObject collider) : base(needle.transform) {
        GeneralItem = needle;
        attached = new AttachedObject();
        this.Collider = collider;
    }

    protected override InteractState AttachState => throw new System.NotImplementedException();

    public override void ConnectItem(Interactable interactable) {
        throw new System.NotImplementedException();
    }

    public override void OnReleaseItem() {
        throw new System.NotImplementedException();
    }

    protected override void AttachEvents(GameObject intObject) {
        throw new System.NotImplementedException();
    }

    protected override void SetInteractors() {
        throw new System.NotImplementedException();
    }

    protected override void SnapObjectPosition() {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
