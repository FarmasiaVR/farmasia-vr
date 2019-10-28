using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLock : MonoBehaviour {

    #region Fields
    [SerializeField]
    [Tooltip("Set camera object here!")]
    private GameObject camera;
    private Vector3 offset;
    #endregion
    // Start is called before the first frame update
    void Start() {
        offset = transform.localPosition;
    }

    // Update is called once per frame
    void Update() {

        transform.LookAt(camera.transform);
        Vector3 rot = transform.eulerAngles;
        Logger.Print(rot);
        if (rot.x > 70 && rot.x < 200) {
            transform.eulerAngles = new Vector3(70, rot.y, rot.z);
        } else if (rot.x < 330 && rot.x > 200) {
            transform.eulerAngles = new Vector3(330, rot.y, rot.z);
        }
    }
}
