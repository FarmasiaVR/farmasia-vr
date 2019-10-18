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
    public GameObject playerLocalPoint;
    [SerializeField]
    public GameObject passThroughTriggerLocal;
    private PassThroughCabinet cabinet;
    private GameObject player;
    #endregion

    private void Start() {
        cabinet = GameObject.FindGameObjectWithTag("PassThrough (Prep)")?.GetComponent<PassThroughCabinet>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void teleportPlayerAndPassthroughCabinet() {
        if (playerTele == null || passThroughTele == null) {
            Logger.Print("Cannot teleport without references to teleportation spots!");
            return;
        }
        foreach (GameObject cabinetItem in cabinet.GetContainedItems()) {
            Vector3 lepos = passThroughTele.transform.position + (cabinetItem.transform.position - passThroughTriggerLocal.transform.position);
            float rotDelta = Quaternion.Angle(passThroughTriggerLocal.transform.rotation, passThroughTele.transform.rotation);
            cabinetItem.transform.position = lepos;
            cabinetItem.transform.RotateAround(passThroughTele.transform.position, passThroughTele.transform.up, rotDelta);

        }
        Vector3 playerLocalPosition = player.transform.InverseTransformPoint(playerLocalPoint.transform.position);
        player.transform.position = new Vector3(playerTele.transform.position.x + playerLocalPosition.x, playerTele.transform.position.y + playerLocalPosition.y, playerTele.transform.position.z + playerLocalPosition.z);
    }
}
