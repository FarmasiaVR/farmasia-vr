using System;
using UnityEngine;
using Valve.VR;

public class TestHandMover : MonoBehaviour {

    private const KeyCode ACTIVATE = KeyCode.Space;
    private const KeyCode USE_CAMERA = KeyCode.K;
    private const KeyCode USE_RIGHT = KeyCode.L;
    private const KeyCode USE_LEFT = KeyCode.J;
    private const KeyCode HAND_INTERACT = KeyCode.Mouse1;
    private const KeyCode MOVE_FORWARD = KeyCode.W;
    private const KeyCode MOVE_LEFT = KeyCode.A;
    private const KeyCode MOVE_BACKWARDS = KeyCode.S;
    private const KeyCode MOVE_RIGHT = KeyCode.D;
    private const KeyCode MOVE_UP = KeyCode.E;
    private const KeyCode MOVE_DOWN = KeyCode.Q;
    private const KeyCode FILL_PASSTHROUGH_CABINET = KeyCode.F;
    private const KeyCode ENABLE_MOUSECONTROL = KeyCode.Minus;

    private enum ControlState {
        HAND_LEFT, HAND_RIGHT, CAMERA
    }

    private ControlState currentState = ControlState.CAMERA;

    private float speedMovement = 1;
    private float speedVertical = 1;
    private float speedHorizontal = 1;

    // Camera rotation
    private float yaw = 0;
    private float pitch = 0;

    private Transform right, left, cam;

    private bool active;
    private bool grabbing;

    [SerializeField]
    private GameObject correctPassthroughCabinetItems;
    private bool mouseControl = false;

    private void Start() {
#if UNITY_EDITOR
#else
        Destroy(this);
#endif
        right = transform.GetChild(1);
        left = transform.GetChild(0);
        cam = transform.GetChild(2);
    }

    private void Update() {
        if (active) {
            CheckStateChange();
            UpdateState();
            UpdateMovement();
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                G.Instance.Progress.CurrentPackage.CurrentTask.ForceClose(false);
            }
        } else if (Input.GetKeyDown(ACTIVATE)) {
            Logger.Print("Activating test hand mover");
            active = true;
            Cursor.lockState = currentState == ControlState.CAMERA ? CursorLockMode.Locked : CursorLockMode.None;
            Destroy(transform.GetComponent<SteamVR_PlayArea>());
            Destroy(right.GetComponent<SteamVR_Behaviour_Pose>());
            Destroy(left.GetComponent<SteamVR_Behaviour_Pose>());
        }
    }

    private void CheckStateChange() {
        if (JustPressed(USE_LEFT)) {
            SetState(ControlState.HAND_LEFT);
        } else if (JustPressed(USE_RIGHT)) {
            SetState(ControlState.HAND_RIGHT);
        } else if (JustPressed(USE_CAMERA)) {
            SetState(ControlState.CAMERA);
        }
        if (JustPressed(FILL_PASSTHROUGH_CABINET)) {
            correctPassthroughCabinetItems.SetActive(true);
        }
    }

    private void UpdateState() {
        if (currentState == ControlState.HAND_LEFT || currentState == ControlState.HAND_RIGHT) {
            UpdateHands();
        } else if (currentState == ControlState.CAMERA) {
            UpdateCamera();
        }
    }

    private void UpdateHands() {
        if (Input.GetMouseButtonDown(0)) {
            if (grabbing) {
                grabbing = false;
                PassGrabInput();
                PassUngrabInput();
            } else {
                grabbing = true;
                PassGrabInput();
            }
        }
        if (JustPressed(HAND_INTERACT)) {
            PassInteractInput();
        }
    }

    private void UpdateCamera() {
        yaw += speedHorizontal * Input.GetAxis("Mouse X");
        pitch -= speedVertical * Input.GetAxis("Mouse Y");
        GetCurrentTransform().eulerAngles = new Vector3(pitch, yaw, 0);
    }

    private void UpdateMovement() {
        Vector3 movement = Vector3.zero;
        if (JustPressed(ENABLE_MOUSECONTROL)) {
            mouseControl = !mouseControl;
            if (mouseControl) {
                Cursor.lockState = CursorLockMode.Locked;
            } else {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        if (mouseControl) {
            movement.x += Input.GetAxis("Mouse X");
            movement.z += Input.GetAxis("Mouse Y");
        } else {
            if (IsPressed(MOVE_FORWARD)) {
                movement.z++;
            }
            if (IsPressed(MOVE_BACKWARDS)) {
                movement.z--;
            }

            if (IsPressed(MOVE_RIGHT)) {
                movement.x++;
            }
            if (IsPressed(MOVE_LEFT)) {
                movement.x--;
            }
        }
        if (IsPressed(MOVE_UP)) {
            movement.y++;
        }
        if (IsPressed(MOVE_DOWN)) {
            movement.y--;
        }
        GetCurrentTransform().Translate(movement * speedMovement * Time.deltaTime);
    }

    private void SetState(ControlState state) {
        if (state == ControlState.CAMERA) {
            Cursor.lockState = CursorLockMode.Locked;
        } else if (state == ControlState.HAND_LEFT) {
            Cursor.lockState = CursorLockMode.None;
        } else if (state == ControlState.HAND_RIGHT) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            throw new NotImplementedException(string.Format("ControlState not implemented: {0}", state.ToString()));
        }
        currentState = state;
    }

    private void PassGrabInput() {
        VRInput.ControlDown(Controls.Grab, GetHandType());
    }

    private void PassUngrabInput() {
        VRInput.ControlUp(Controls.Grab, GetHandType());
    }

    private void PassInteractInput() {
        VRInput.ControlDown(Controls.GrabInteract, GetHandType());
    }

    private Transform GetCurrentTransform() {
        if (currentState == ControlState.CAMERA) {
            return cam;
        } else if (currentState == ControlState.HAND_LEFT) {
            return left;
        } else if (currentState == ControlState.HAND_RIGHT) {
            return right;
        } else {
            throw new NotImplementedException(string.Format("ControlState not implemented: {0}", currentState.ToString()));
        }
    }

    private SteamVR_Input_Sources GetHandType() {
        if (currentState == ControlState.HAND_LEFT) {
            return SteamVR_Input_Sources.LeftHand;
        } else if (currentState == ControlState.HAND_RIGHT) {
            return SteamVR_Input_Sources.RightHand;
        } else {
            throw new InvalidOperationException("No hand is currently being controlled!");
        }
    }

    private bool IsPressed(KeyCode c) {
        return Input.GetKey(c);
    }

    private bool JustPressed(KeyCode c) {
        return Input.GetKeyDown(c);
    }

    private bool JustReleased(KeyCode c) {
        return Input.GetKeyUp(c);
    }
}
