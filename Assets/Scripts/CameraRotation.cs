using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {
    float horizontalSpeed = 2.0f;
    float verticalSpeed = 2.0f;

    enum Axis { X, Y, Z };

    void Update() {
        if (Input.GetMouseButton(0)) {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(getAxis(Axis.X, -v), getAxis(Axis.Y, h), getAxis(Axis.Z, 0));
        }

    }

    private float getAxis(Axis axe, float rotation) {
        if (axe == Axis.X) {
            return transform.eulerAngles.x + rotation;
        } else if (axe == Axis.Y) {
            return transform.eulerAngles.y + rotation;
        } else if (axe == Axis.Z) {
            return transform.eulerAngles.z + rotation;
        }
        return 0;
    }
}
