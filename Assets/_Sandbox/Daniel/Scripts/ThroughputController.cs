using UnityEngine;

public class ThroughputController : MonoBehaviour {

    #region Fields
    [SerializeField]
    private GameObject source;
    [SerializeField]
    private GameObject destination;

    private PassThroughCabinet srcCabinet;
    private PassThroughCabinet dstCabinet;
    #endregion

    private void Start() {
        srcCabinet = source?.GetComponentInChildren<PassThroughCabinet>();
        dstCabinet = destination?.GetComponentInChildren<PassThroughCabinet>();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            CopyObjects.Copy(source, destination, srcCabinet.objectsInsideArea);
        }
    }
}