﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasketRequiredItems
{
    public static Dictionary<ObjectType, int> ForMembraneFilteration()
    {
        var items = new Dictionary<ObjectType, int>();
        items.Add(ObjectType.Pump, 1);

        return items;
    }
}

public class Basket : MonoBehaviour
{
    private Dictionary<Type, int> missingObjects;
    private bool itemPlaced = false;
    [SerializeField]
    private GameObject childCollider;
    [SerializeField]
    private TriggerInteractableContainer itemContainer;

    private void Start()
    {
        switch (G.Instance.CurrentSceneType)
        {
            case (SceneTypes.MembraneFilteration): missingObjects = GetMembraneFilterationItems(); break;
        }

        itemContainer = childCollider.gameObject.AddComponent<TriggerInteractableContainer>();
        itemContainer.OnEnter = EnterBasket;
    }

    private Dictionary<Type, int> GetMembraneFilterationItems()
    {
        var items = new Dictionary<Type, int>();
        items.Add(typeof(Pump), 1);
        return items;
    }

    private void EnterBasket(Interactable other)
    {
        GeneralItem item = other as GeneralItem;
        if (item == null)
        {
            return;
        }
        if (!itemPlaced)
        {
            Events.FireEvent(EventType.ItemPlacedForReference, CallbackData.Object(this));
            itemPlaced = true;
        }
        Events.FireEvent(EventType.ItemPlacedInBasket, CallbackData.Object(item));
        if (missingObjects.ContainsKey(item.GetType()))
            missingObjects[item.GetType()]--;
    }
    public List<Interactable> GetBasketItems()
    {
        return itemContainer.Objects.ToList();
    }
}