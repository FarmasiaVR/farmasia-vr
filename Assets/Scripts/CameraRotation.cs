using UnityEngine;

public class CameraRotation : MonoBehaviour {

    #region Constants
    private const float HORIZONTAL_SPEED = 2.0f;
    private const float VERTICAL_SPEED = 2.0f;
    #endregion

    private enum Axis { X, Y, Z };

    private void Update() {
        if (Input.GetMouseButton(0)) {
            float h = HORIZONTAL_SPEED * Input.GetAxis("Mouse X");
            float v = VERTICAL_SPEED * Input.GetAxis("Mouse Y");
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
