using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An item that can take an attachment.
/// Requires <c>RigidBody</c> and <c>SphereCollider</c> at GameObject root and <c>LineRenderer</c> at child-index 1
/// </summary>
public class ReceiverItem : AttachmentItem
{
    protected AttachmentItem ConnectedItem = null;

    public ObjectType ReceivedObjectType;

    protected LineRenderer LineEffect;
    protected SphereCollider TriggerCollider;
    public bool SlotOccupied = false;

    public HashSet<GameObject> PossibleItems;
    protected GameObject NearestItem = null;

    protected override void Awake() {
        base.Awake();

        PossibleItems = new HashSet<GameObject>();
        LineEffect = transform.GetChild(1).GetComponent<LineRenderer>();
        TriggerCollider = gameObject.GetComponent<SphereCollider>();
    }

    /// <summary>
    /// Calculates distances from <value>PossibleItems</value> and returns the smallest item
    /// </summary>
    /// <returns><c>GameObject</c> with the smallest distance</returns>
    protected GameObject GetNearestItem() {
        float nearestDistance = float.MaxValue;
        GameObject nearestItem = null;
        foreach (GameObject item in PossibleItems) {
            float newDistance = Vector3.Distance(transform.position, item.transform.position);
            if (newDistance < nearestDistance) {
                nearestDistance = newDistance;
                nearestItem = item;
            }
        }
        return nearestItem;
    }

    /// <summary>
    /// Calls <c>ConnectAttachment</c> if any valid object gets close enough (defined by <value>SnapDistance</value>)
    /// </summary>
    protected void LateUpdate() {
        if (PossibleItems.Count > 0) {
            NearestItem = GetNearestItem();
        }

        if (NearestItem != null && !SlotOccupied) {
            if (Vector3.Distance(transform.TransformPoint(TriggerCollider.center), NearestItem.transform.position) <= SnapDistance) {
                ConnectAttachment();
            }
        }

        //UpdateLineEffect(PossibleItems.Count > 0);
    }

    /// <summary>
    /// Updates the <c>LineRenderer</c> endpoint positions between nearest valid object and this GameObject
    /// </summary>
    /// <param name="possibleConnectionExists">Is there a possible item?</param>
    protected void UpdateLineEffect(bool possibleConnectionExists) {
        if (possibleConnectionExists) {
            LineEffect.positionCount = 2;
            LineEffect.SetPosition(0, transform.position);
            LineEffect.SetPosition(1, NearestItem.transform.position);
        } else {
            LineEffect.positionCount = 1;
        }
    }

    /// <summary>
    /// Adds <c>GameObjects</c> to <value>PossibleItems</value> when they enter trigger volume if their <value>ObjectType</value>
    /// equals <value>ReceivedObjectType</value>
    /// </summary>
    /// <param name="other">The entering collider</param>
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.transform.IsChildOf(transform)) {
            return;
        }

        if (!CanConnect(other.GetComponent<Interactable>())) {
            return;
        }

        GeneralItem colliderItem = other.gameObject.GetComponentInParent<GeneralItem>();
        if (!SlotOccupied && !(colliderItem == null) && colliderItem.ObjectType == ReceivedObjectType) {
            PossibleItems.Add(colliderItem.gameObject);
        }
    }

    /// <summary>
    /// Removes item from <value>PossibleItems</value> when they exit trigger volume
    /// </summary>
    /// <param name="other">The exiting collider</param>
    private void OnTriggerExit(Collider other) {
        PossibleItems.Remove(other.gameObject);
        if (PossibleItems.Count == 0) {
            NearestItem = null;
        }
    }

    /// <summary>
    /// Marks this GameObject's attachment slot occupied and begins the attachment sequence for the AttachItem
    /// </summary>
    protected virtual void ConnectAttachment() {
        Logger.Print("connected parts");
        SlotOccupied = true;
        ConnectedItem = NearestItem.GetComponent<AttachmentItem>();

        ConnectedItem.StartCoroutine(ConnectedItem.WaitForHandDisconnectAndConnectItems(this));

    }

    /// <summary>
    /// Disconnects the item in parameter from this item and places it to the scene root.
    /// Releases the grabbing of an empty item the Hand has during the <c>WaitForDistance</c> co-routine 
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="itemToDisconnect"></param>
    public void Disconnect(Hand hand, AttachmentItem itemToDisconnect) {
        ConnectedItem = null;
        itemToDisconnect.transform.SetParent(null);
        itemToDisconnect.transform.position = hand.transform.position;

        itemToDisconnect.RigidbodyContainer.Enable();

        hand.GrabUninteract();
    }
}