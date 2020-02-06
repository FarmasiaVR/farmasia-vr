using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupObject : MonoBehaviour {

    [SerializeField]
    private Collider coll;

    [SerializeField]
    private CabinetBase laminarCabinet, secondPassThroughCabinet;

    private TriggerInteractableContainer roomItems;

    private bool startedCleanup;
    private bool finished;

    private void Start() {
        
        roomItems = coll.gameObject.AddComponent<TriggerInteractableContainer>();
        Events.SubscribeToEvent(ItemLiftedFromFloor, this, EventType.ItemLiftedOffFloor);
        Events.SubscribeToEvent(ItemDroppedInTrash, this, EventType.ItemDroppedInTrash);
    }

    public static CleanupObject GetCleanup() {
        return GameObject.FindObjectOfType<CleanupObject>();
    }

    private void ItemLiftedFromFloor(CallbackData data) {
        GeneralItem item = (GeneralItem)data.DataObject;

        if (G.Instance.Progress.CurrentPackage.name == PackageName.EquipmentSelection) {
            return;
        }

        if (!startedCleanup && !item.IsClean) {
            TaskBase.CreateTaskMistakeGlobal(TaskType.ScenarioOneCleanUp, "Siivoa lattialla olevat esineet vasta lopuksi", 1);
        }
    }
    private void ItemDroppedInTrash(CallbackData data) {
        GeneralItem g = (GeneralItem)data.DataObject;

        if (G.Instance.Progress.CurrentPackage.name == PackageName.EquipmentSelection) {
            return;
        }

        if (g.ObjectType == ObjectType.Bottle) {
            TaskBase.CreateTaskMistakeGlobal(TaskType.ScenarioOneCleanUp, "Pulloa ei saa heittää roskikseen", 1);
        }
        if (g.ObjectType == ObjectType.SterileBag) {
            TaskBase.CreateTaskMistakeGlobal(TaskType.ScenarioOneCleanUp, "Steriilipussia ei saa heittää roskikseen", 1);
        }
    }

    private void Update() {
        if (startedCleanup && !finished) {
            if (RoomGeneralItemCount() <= 1) {
                finished = true;
                Logger.Warning("Finishing cleanup");
                G.Instance.Progress.ForceCloseTask(TaskType.ScenarioOneCleanUp, false);
            }
        }
    }

    private int RoomGeneralItemCount() {
        int count = 0;
        foreach (Interactable i in roomItems.Objects) {
            if (i as GeneralItem is var g && g != null) {
                if (g.ObjectType == ObjectType.Bottle || g.ObjectType == ObjectType.SterileBag) {
                    continue;
                }
                if (g.Rigidbody == null || g.Rigidbody.isKinematic) {
                    continue;
                }
                count++;
            }
        }

        return count;
    }

    public void EnableCleanup() {
        ObjectFactory.DestroyAllFactories(true);

        foreach (Interactable i in secondPassThroughCabinet.GetContainedItems()) {
            i.DestroyInteractable();
        }

        foreach (ItemSpawner i in GameObject.FindObjectsOfType<ItemSpawner>()) {
            Destroy(i.gameObject);
        }

        startedCleanup = true;
    }
}
