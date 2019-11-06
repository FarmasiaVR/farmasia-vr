using UnityEngine;

public class RoomTeleport : MonoBehaviour {
    #region Fields
    [Tooltip("Insert Teleportation Spot for the player here!")]
    [SerializeField]
    public GameObject playerTele;
    [Tooltip("Insert Pass-Through cabinet teleportation here!")]
    [SerializeField]
    public GameObject passThroughTele;
    [SerializeField]
    public GameObject passThroughTriggerLocal;
    private CabinetBase cabinet;
    private GameObject player;
    #endregion

    private void Start() {
        cabinet = GameObject.FindGameObjectWithTag("PassThrough (Prep)")?.GetComponent<CabinetBase>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Teleports player and Contents of Pass-Through cabinet to the next room.
    /// </summary>
    public void TeleportPlayerAndPassthroughCabinet() {
        if (playerTele == null || passThroughTele == null) {
            Logger.Print("Cannot teleport without references to teleportation spots!");
            return;
        }
        foreach (GameObject cabinetItem in cabinet.GetContainedItems()) {
            Vector3 newPosition = passThroughTele.transform.position + (cabinetItem.transform.position - passThroughTriggerLocal.transform.position);
            float rotDelta = Quaternion.Angle(passThroughTriggerLocal.transform.rotation, passThroughTele.transform.rotation);
            cabinetItem.transform.position = newPosition;
            cabinetItem.transform.RotateAround(passThroughTele.transform.position, passThroughTele.transform.up, rotDelta);
        }
        player.transform.position = new Vector3(playerTele.transform.position.x, playerTele.transform.position.y, playerTele.transform.position.z);
    }
}
