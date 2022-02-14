using System.Collections.Generic;
using UnityEngine;

public class RoomTeleport : MonoBehaviour {

    #region Fields
    [Tooltip("VRPlayer instance")]
    [SerializeField]
    private Transform player;
    [Tooltip("Player teleportation destination")]
    [SerializeField]
    public Transform playerDst;

    [Tooltip("Passthrough teleportation source")]
    [SerializeField]
    public Transform passthroughSrc;
    [Tooltip("Passthrough teleportation destination")]
    [SerializeField]
    public Transform passthroughDst;
    #endregion

    /// <summary>
    /// Teleports player and Contents of Pass-Through cabinet to the next room.
    /// </summary>
    public void TeleportPlayerAndPassthroughCabinet() {
        if (playerDst == null || passthroughDst == null) {
            Logger.Print("Cannot teleport without references to teleportation spots!");
            return;
        }

        CabinetBase cabinet = passthroughSrc.GetComponent<CabinetBase>();
        List<Transform> items = cabinet.GetContainedItems().ConvertAll(obj => obj.transform);
        foreach (Transform item in items) {
            float rotDelta = Quaternion.Angle(passthroughSrc.rotation, passthroughDst.rotation);
            item.position = passthroughDst.position + (item.position - passthroughSrc.position);
            item.RotateAround(passthroughDst.position, passthroughDst.up, rotDelta);
            CreateSpawner(item);
        }

        foreach (VRActionsMapper h in VRInput.Hands) {
            h.Hand.GrabUninteract();
            h.Hand.Uninteract();
        }

        player.position = playerDst.position;

        MedicinePreparationScene m = G.Instance.Scene as MedicinePreparationScene;

        if (!m.Restarted || MedicinePreparationScene.SavedScoreState == null) {
            m.SaveProgress();
        }
    }

    private void CreateSpawner(Transform item) {
        GameObject obj = new GameObject();
        obj.transform.SetPositionAndRotation(item.position, item.rotation);
        obj.AddComponent<ItemSpawner>();
        obj.GetComponent<ItemSpawner>().SetCopyObject(item.gameObject);
    }
}