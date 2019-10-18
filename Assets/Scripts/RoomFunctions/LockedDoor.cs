using System;
using UnityEngine;

public class LockedDoor : MonoBehaviour {

    private void Start() {
    }

    public void CheckExitPermission() {
        GameObject gm = GameObject.FindWithTag("PassThroughCabinet");
        Events.FireEvent(EventType.CorrectItemsInThroughput, CallbackData.Object(gm.GetComponent<CabinetBase>().objectsInsideArea));
        if (String.Equals(G.Instance.Progress.currentPackage.name, "Workspace")) {
            Events.FireEvent(EventType.CorrectLayoutInThroughput, CallbackData.String("" + gm.GetComponent<CabinetBase>().objectsInsideArea.Count));
            //move to second room
        } else {
            UISystem.Instance.CreatePopup(gm.GetComponent<CabinetBase>().GetMissingItems(), MessageType.Notify);
        }
    }
}