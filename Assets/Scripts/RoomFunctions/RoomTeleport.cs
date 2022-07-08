using System.Collections.Generic;
using UnityEngine;

public class RoomTeleport : MonoBehaviour {

    public Transform player;
    public Transform playerDst;
    public Transform passthroughSrc;
    public Transform passthroughDst;

    /// <summary>
    /// Teleports player and contents of pass-through cabinet to the next room
    /// </summary>
    public void TeleportPlayerAndPassthroughCabinet() {
        if (playerDst == null || passthroughDst == null) {
            Logger.Print("Cannot teleport without references to teleportation spots!");
            return;
        }

        if (G.Instance.CurrentSceneType != SceneTypes.MembraneFilteration) TeleportItems();

        TeleportPlayer();

        if (G.Instance.CurrentSceneType != SceneTypes.MedicinePreparation) return;

        MedicinePreparationScene m = G.Instance.Scene as MedicinePreparationScene;

        if (!m.Restarted || MedicinePreparationScene.SavedScoreState == null) {
            m.SaveProgress();
        }
    }

    public void TeleportPlayer() {
        foreach (VRActionsMapper h in VRInput.Hands) {
            h.Hand.GrabUninteract();
            h.Hand.Uninteract();
        }

        player.position = playerDst.position;
    }

    private void TeleportItems() {
        CabinetBase cabinet = passthroughSrc.GetComponent<CabinetBase>();
        List<Transform> items = cabinet.GetContainedItems().ConvertAll(obj => obj.transform);
        foreach (Transform item in items) {
            float rotDelta = Quaternion.Angle(passthroughSrc.rotation, passthroughDst.rotation);
            item.position = passthroughDst.position + (item.position - passthroughSrc.position);
            item.RotateAround(passthroughDst.position, passthroughDst.up, rotDelta);
            CreateSpawner(item);
        }
    }

    private void CreateSpawner(Transform item) {
        GameObject obj = new GameObject();
        obj.transform.SetPositionAndRotation(item.position, item.rotation);
        obj.AddComponent<ItemSpawner>();
        obj.GetComponent<ItemSpawner>().SetCopyObject(item.gameObject);
    }
}
