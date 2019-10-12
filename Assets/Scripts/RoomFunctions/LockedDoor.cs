using System;
using UnityEngine;

public class LockedDoor : MonoBehaviour {

    private void Start() {
    }

    public void CheckExitPermission() {
        GameObject gm = GameObject.FindWithTag("PassThroughCabinet");
        Events.FireEvent(EventType.ArrangedItemsInThroughput, CallbackData.String("" + gm.GetComponent<PassThroughCabinet>().objectsInsideArea.Count));
        if (String.Equals(G.Instance.Progress.currentPackage.name, "Workspace")) {
            //move to second room
        } else {
            UISystem.Instance.CreatePopup(gm.GetComponent<PassThroughCabinet>().GetMissingItems(), MessageType.Notify);
        }
    }
}