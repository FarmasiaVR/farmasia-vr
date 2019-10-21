using UnityEngine;

public class ThroughputController : MonoBehaviour {

    #region Fields
    [SerializeField]
    private GameObject source;
    [SerializeField]
    private GameObject destination;

    private CabinetBase srcCabinet;
    private CabinetBase dstCabinet;
    #endregion

    private void Start() {
        srcCabinet = source?.GetComponentInChildren<CabinetBase>();
        dstCabinet = destination?.GetComponentInChildren<CabinetBase>();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            CopyObjects.Copy(source, destination, srcCabinet.objectsInsideArea);
        }
    }
}