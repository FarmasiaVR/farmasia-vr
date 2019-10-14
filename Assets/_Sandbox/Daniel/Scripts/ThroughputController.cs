using UnityEngine;

public class ThroughputController : MonoBehaviour {

    #region Fields
    [SerializeField]
    private GameObject source;
    [SerializeField]
    private GameObject destination;

    private ThroughputCabinet srcCabinet;
    private ThroughputCabinet dstCabinet;
    #endregion

    private void Start() {
        srcCabinet = source?.GetComponentInChildren<ThroughputCabinet>();
        dstCabinet = destination?.GetComponentInChildren<ThroughputCabinet>();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            srcCabinet?.TransferObjectsTo(dstCabinet);
        }
    }
}