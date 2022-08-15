using System.Collections.Generic;
using UnityEngine;

public class RoomTeleport : MonoBehaviour {

    public Transform player;
    public Transform playerDestination;

    /// <summary>
    /// Teleports player to the next room
    /// </summary>
    public void TeleportPlayer() {
        if (playerDestination == null) {
            Logger.Error("Player teleportation spot missing");
            return;
        }

        // Making sure we don't bring any grabbed items with us when teleporting
        foreach (VRActionsMapper hand in VRInput.Hands) {
            hand.Hand.GrabUninteract();
            hand.Hand.Uninteract();
        }

        player.position = playerDestination.position;

        /*MedicinePreparationScene m = G.Instance.Scene as MedicinePreparationScene;
        if (!m.Restarted || MedicinePreparationScene.SavedScoreState == null) {
            m.SaveProgress();
        }*/
    }
}
