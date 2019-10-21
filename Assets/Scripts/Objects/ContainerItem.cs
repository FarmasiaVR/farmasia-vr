using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerItem {

    #region fields
    private LiquidContainer container;
    private GeneralItem item;

    private Dictionary<int, int> enteredObjects;
    #endregion

    public ContainerItem(LiquidContainer container, GeneralItem item) {
        this.container = container;
        this.item = item;
        enteredObjects = new Dictionary<int, int>();
    }

    public void TriggerEnter(Collider c) {
        
        Interactable interactable = Interactable.GetInteractable(c.transform);

        if (interactable == null) {
            return;
        }

        Logger.Print("Liquid container enter: " + c.gameObject.name);

        AddToDictionary(interactable);

        Syringe syringe = interactable as Syringe;

        if (syringe == null) {
            Logger.Print("No syringe");
            return;
        }

        if (item.ObjectType == ObjectType.Bottle) {
            syringe.State.On(InteractState.InBottle);
        }

        Logger.Print("In syringe");

        syringe.BottleContainer = container;
    }
    public void TriggerExit(Collider c) {
        Interactable interactable = Interactable.GetInteractable(c.transform);

        if (interactable == null) {
            return;
        }

        Logger.Print("Liquid container exit: " + c.gameObject.name);

        bool exited = RemoveFromDictionary(interactable);

        Syringe syringe = interactable as Syringe;

        if (syringe == null) {
            return;
        }

        if (item.ObjectType == ObjectType.Bottle && exited) {
            syringe.State.Off(InteractState.InBottle);
            Logger.Print("Syringe exited bottle");
            syringe.BottleContainer = null;
            //test event trigger
            // Events.FireEvent(EventType.MedicineToSyringe, CallbackData.Object(syringe));
        }
    }

    private void AddToDictionary(Interactable interactable) {

        int id = interactable.GetInstanceID();

        if (enteredObjects.ContainsKey(id)) {
            enteredObjects[id]++;
        } else {
            enteredObjects.Add(id, 1);
        }
    }
    private bool RemoveFromDictionary(Interactable interactable) {
        int id = interactable.GetInstanceID();

        if (enteredObjects.ContainsKey(id)) {
            enteredObjects[id]--;

            if (enteredObjects[id] == 0) {
                enteredObjects.Remove(id);
                return true;
            }
        } else {
            Logger.Warning("Object exited invalid amount of times");
        }

        return false;
    }
}
