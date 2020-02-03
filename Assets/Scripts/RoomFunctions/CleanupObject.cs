using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupObject : MonoBehaviour {

    [SerializeField]
    private Collider coll;

    [SerializeField]
    private CabinetBase laminarCabinet, secondPassThroughCabinet;

    private TriggerInteractableContainer roomItems;

    private bool started;
    private bool finished;

    private void Start() {
        roomItems = coll.gameObject.AddComponent<TriggerInteractableContainer>();
    }

    public static CleanupObject GetCleanup() {
        return GameObject.FindObjectOfType<CleanupObject>();
    }

    private void Update() {
        if (started && !finished) {
            if (RoomGeneralItemCount() == 0) {
                finished = true;
                Logger.Warning("Finishing cleanup");
                G.Instance.Progress.ForceCloseTask(TaskType.ScenarioOneCleanUp);
            }
        }
    }

    private int RoomGeneralItemCount() {
        int count = 0;
        foreach (Interactable i in roomItems.Objects) {
            if (i as GeneralItem != null) {
                // Logger.Print("Clean up item: " + count + ", " + i.name);
                count++;
            }
        }
        return count;
    }

    public void EnableCleanup() {

        laminarCabinet.DisableCapFactory();

        foreach (Interactable i in secondPassThroughCabinet.GetContainedItems()) {
            i.DestroyInteractable();
        }

        started = true;
    }
}
