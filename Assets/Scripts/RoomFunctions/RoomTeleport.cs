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
        }
        player.position = playerDst.position;// new Vector3(playerDst.position.x, playerDst.position.y, playerDst.position.z);
    }
}
