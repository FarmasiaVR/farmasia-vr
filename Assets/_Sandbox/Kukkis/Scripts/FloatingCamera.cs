using UnityEngine;

public class FloatingCamera : MonoBehaviour {

    private float speed = 10.0f;
    private Rigidbody body;
    private ForceMode mode = ForceMode.Acceleration;
    private float horizontalSpeed = 2.0f;
    private float verticalSpeed = 2.0f;

    private enum Axis { X, Y, Z };
    private enum Controls {
        Left = KeyCode.A,
        Right = KeyCode.D,
        Up = KeyCode.E,
        Down = KeyCode.Q,
        Forward = KeyCode.W,
        Backward = KeyCode.S
    };
    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKey((KeyCode) Controls.Left)) {
            DoMovement(Controls.Left);
        }
        if (Input.GetKey((KeyCode)Controls.Right)) {
            DoMovement(Controls.Right);
        }
        if (Input.GetKey((KeyCode)Controls.Up)) {
            DoMovement(Controls.Up);
        }
        if (Input.GetKey((KeyCode)Controls.Down)) {
            DoMovement(Controls.Down);
        }
        if (Input.GetKey((KeyCode)Controls.Forward)) {
            DoMovement(Controls.Forward);
        }
        if (Input.GetKey((KeyCode)Controls.Backward)) {
            DoMovement(Controls.Backward);
        }

        if (Input.GetMouseButton(0)) {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(getAxis(Axis.X, -v), getAxis(Axis.Y, h), getAxis(Axis.Z, 0));
        }
    }


    private void DoMovement(Controls key) {
        switch (key) {
            case Controls.Left:
                body.AddRelativeForce((new Vector3(-1, 0, 0) * speed), mode);
                return;
            case Controls.Right:
                body.AddRelativeForce((new Vector3(1, 0, 0) * speed), mode);
                return;
            case Controls.Up:
                body.AddRelativeForce((new Vector3(0, 1, 0) * speed), mode);
                return;
            case Controls.Down:
                body.AddRelativeForce((new Vector3(0, -1, 0) * speed), mode);
                return;
            case Controls.Forward:
                body.AddRelativeForce((new Vector3(0, 0, 1) * speed), mode);
                return;
            case Controls.Backward:
                body.AddRelativeForce((new Vector3(0, 0, -1) * speed), mode);
                return;
            default:
                return;
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
