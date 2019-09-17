//HUOM! 1. versio, ei lopullinen -> uusi versio kun VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    #region fields
    private Transform block;
    #endregion

    // Start is called before the first frame update
    void Start() {
        // get block
        block = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(block.position.x, transform.position.y, block.position.z);
    }
}
