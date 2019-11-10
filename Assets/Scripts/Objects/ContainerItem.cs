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

        AddToDictionary(interactable);

        Syringe syringe = interactable as Syringe;

        if (syringe == null) {
            return;
        }

        if (item.ObjectType == ObjectType.Bottle) {
            syringe.State.On(InteractState.InBottle);
            syringe.hasBeenInBottle = true;
            Events.FireEvent(EventType.SyringeToMedicineBottle, CallbackData.Object(syringe));
            Events.FireEvent(EventType.Disinfect, CallbackData.Object(item));
        }

        syringe.BottleContainer = container;
    }
    public void TriggerExit(Collider c) {
        Interactable interactable = Interactable.GetInteractable(c.transform);

        if (interactable == null) {
            return;
        }

        bool exited = RemoveFromDictionary(interactable);

        Syringe syringe = interactable as Syringe;

        if (syringe == null) {
            return;
        }

        if (item.ObjectType == ObjectType.Bottle && exited) {
            syringe.State.Off(InteractState.InBottle);
            syringe.BottleContainer = null;
            Events.FireEvent(EventType.SyringeFromMedicineBottle, CallbackData.Object(syringe));
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
