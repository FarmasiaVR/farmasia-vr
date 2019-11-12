using System;
using UnityEngine;

public class LockedDoor : MonoBehaviour {

    private void Start() {
    }

    public void CheckExitPermission() {
        GameObject gm = GameObject.FindWithTag("PassThroughCabinet");
        Events.FireEvent(EventType.CorrectItemsInThroughput, CallbackData.Object(gm.GetComponent<CabinetBase>().objectsInsideArea));
        if (G.Instance.Progress.IsCurrentPackage(PackageName.Workspace)) {
            Events.FireEvent(EventType.CorrectLayoutInThroughput, CallbackData.String("" + gm.GetComponent<CabinetBase>().objectsInsideArea.Count));
        }
    }
}