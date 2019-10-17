using UnityEngine;

public class RoomTeleport : MonoBehaviour {
    #region Fields
    [Tooltip("Insert Teleportation Spot for the player here!")]
    [SerializeField]
    readonly GameObject playerTele;
    [Tooltip("Insert Pass-Through cabinet teleportation here!")]
    [SerializeField]
    readonly GameObject passThroughTele;

    private PassThroughCabinet cabinet;
    private GameObject player;
    #endregion

    /// <summary>
    /// Moves player and PassThrough cabinet to next room.
    /// </summary>
    public RoomTeleport() {
        cabinet = GameObject.FindGameObjectWithTag("PassThrough (Prep)")?.GetComponent<PassThroughCabinet>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void teleportPlayerAndPassthroughCabinet() {
        if (playerTele == null || passThroughTele == null) {
            Logger.Print("Cannot teleport without references to teleportation spots!");
            return;
        }
        foreach (GameObject cabinetItem in cabinet.GetContainedItems()) {
            Vector3 localPosition = cabinetItem.transform.InverseTransformPoint(passThroughTele.transform.position);
            cabinetItem.transform.position = new Vector3(passThroughTele.transform.position.x + localPosition.x, passThroughTele.transform.position.y + localPosition.y, passThroughTele.transform.position.z + localPosition.z);
        }
        Vector3 playerLocalPosition = player.transform.InverseTransformPoint(playerTele.transform.position);
        player.transform.position = new Vector3(playerTele.transform.position.x + playerLocalPosition.x, playerTele.transform.position.y + playerLocalPosition.y, playerTele.transform.position.z + playerLocalPosition.z);
    }
}
