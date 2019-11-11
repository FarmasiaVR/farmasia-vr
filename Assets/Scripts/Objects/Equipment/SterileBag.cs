using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SterileBag : GeneralItem {

    #region fields
    public List<GameObject> objectsInBag;
    public bool IsClosed { get; private set; }
    public bool IsSterile { get; private set; }
    private bool itemPlaced = false;
    [SerializeField]
    private GameObject childCollider;
    #endregion
    
    // Start is called before the first frame update
    protected override void Start_GeneralItem() {
        ObjectType = ObjectType.SterileBag;
        IsClean = true;

        objectsInBag = new List<GameObject>();
        IsClosed = false;
        IsSterile = true;
        
        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => EnterSterileBag(collider)));
        CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnExit(collider => ExitSterileBag(collider)));
    }

    private void EnterSterileBag(Collider other) {
        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (item == null || IsClosed) {
            return;
        }
        if (!itemPlaced) {
            Events.FireEvent(EventType.ItemPlacedInSterileBag, CallbackData.Object(this));
            itemPlaced = true;
        }
        
        if (!objectsInBag.Contains(foundObject)) {
            objectsInBag.Add(foundObject);
            Events.FireEvent(EventType.SterileBag, CallbackData.Object(objectsInBag));
            if (!item.IsClean) {
                IsSterile = false;
            }
        }
    }

    private void ExitSterileBag(Collider other) {
        GameObject foundObject = Interactable.GetInteractableObject(other.transform);
        GeneralItem item = foundObject?.GetComponent<GeneralItem>();
        if (!IsClosed && item != null) {
            objectsInBag.Remove(foundObject);
        }
    }

    private void CloseSterileBag() {
        IsClosed = true;
        Events.FireEvent(EventType.SterileBag, CallbackData.Object(objectsInBag));
    }
}