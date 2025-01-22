using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Basket : MonoBehaviour {

    private TriggerInteractableContainer itemContainer;
    private bool itemPlaced = false;

    public GameObject childCollider;

    private void Start() {
        itemContainer = childCollider.AddComponent<TriggerInteractableContainer>();
        itemContainer.OnEnter = EnterBasket;
    }

    private void EnterBasket(Interactable other) {
        GeneralItem item = other as GeneralItem;
        if (item == null) {
            return;
        }
        if (!itemPlaced) {
            Events.FireEvent(EventType.ItemPlacedBasketForReference, CallbackData.Object(this));
            itemPlaced = true;
        }
        Events.FireEvent(EventType.ItemPlacedInBasket, CallbackData.Object(item));
    }

    public List<Interactable> GetBasketItems() {
        return itemContainer.Objects.ToList();
    }
}
