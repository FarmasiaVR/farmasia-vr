using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaminarCabinetTable : MonoBehaviour {

    #region fields
    private Dictionary<GeneralItem, float> itemTimes;
    private HashSet<GeneralItem> clothItems;

    [SerializeField]
    private GameObject cloth;

    private static float ContaminateTime = 2.5f;
    #endregion

    private void Start() {
        itemTimes = new Dictionary<GeneralItem, float>();
        clothItems = new HashSet<GeneralItem>();

        if (cloth == null) {
            Logger.Warning("No cloth assigned, Laminar cabinet won't contaminate objects");
            Destroy(this);
            return;
        }

        CollisionSubscription.SubscribeToCollision(cloth, new CollisionListener().OnEnter(ClothOnCollisionEnter).OnExit(ClothOnCollisionExit));
    }

    #region Collision events
    private void OnCollisionEnter(Collision collision) {
        GeneralItem item = Interactable.GetInteractableObject(collision.gameObject.transform).GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }

        if (itemTimes.ContainsKey(item)) {
            itemTimes[item] = 0;
        } else {
            itemTimes.Add(item, 0);
        }
    }
    private void OnCollisionStay(Collision collision) {
        GeneralItem item = Interactable.GetInteractableObject(collision.gameObject.transform).GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }

        if (!itemTimes.ContainsKey(item)) {
            Logger.Error("Item not in dictionary");
            return;
        }

        float time = itemTimes[item];

        if (time < 0) {
            return;
        }

        time += Time.deltaTime;

        if (time > ContaminateTime) {
            if (ContaminateItem(item)) {
                itemTimes[item] = -1;
            } else {
                itemTimes[item] = 0;
            }
        } else {
            itemTimes[item] = time;
        }
    }

    private void OnCollisionExit(Collision collision) {
        GeneralItem item = Interactable.GetInteractableObject(collision.gameObject.transform).GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }

        if (itemTimes.ContainsKey(item)) {
            itemTimes.Remove(item);
        }
    }
    #endregion

    #region Cloth events
    private void ClothOnCollisionEnter(Collision collision) {
        GeneralItem item = Interactable.GetInteractableObject(collision.gameObject.transform).GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }

        clothItems.Add(item);
    }

    private void ClothOnCollisionExit(Collision collision) {
        GeneralItem item = Interactable.GetInteractableObject(collision.gameObject.transform).GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }

        clothItems.Remove(item);
    }
    #endregion
    private bool ContaminateItem(GeneralItem item) {
        if (clothItems.Contains(item)) {
            return false;
        }

        item.IsClean = false;
        UISystem.Instance.CreatePopup("Työvälineet eivät saisi koskea laminaarikaapin pintaa.", MsgType.Mistake);

        return true;
    }
}
