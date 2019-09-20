//HUOM! 1. versio, ei lopullinen -> uusi versio kun VR
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    #region fields
    private Transform block;
    #endregion

    void Start() {
        block = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update() {
        transform.position = new Vector3(block.position.x, transform.position.y, block.position.z);
    }
}
