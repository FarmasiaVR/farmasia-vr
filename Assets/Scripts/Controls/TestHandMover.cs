using System;
using UnityEngine;
using Valve.VR;

public class TestHandMover : MonoBehaviour {

    #region fields
    private const KeyCode ACTIVATE = KeyCode.Space;
    private const KeyCode USE_CAMERA = KeyCode.K;
    private const KeyCode USE_RIGHT = KeyCode.L;
    private const KeyCode USE_LEFT = KeyCode.J;
    private const KeyCode HAND_GRAB = KeyCode.Mouse0;
    private const KeyCode HAND_INTERACT = KeyCode.Mouse1;
    private const KeyCode MOVE_FORWARD = KeyCode.W;
    private const KeyCode MOVE_LEFT = KeyCode.A;
    private const KeyCode MOVE_BACKWARDS = KeyCode.S;
    private const KeyCode MOVE_RIGHT = KeyCode.D;
    private const KeyCode MOVE_UP = KeyCode.E;
    private const KeyCode MOVE_DOWN = KeyCode.Q;

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
    #endregion

    private void Start() {
        right = transform.GetChild(1);
        left = transform.GetChild(0);
        cam = transform.GetChild(2);
    }

    private void Update() {
        if (active) {
            CheckStateChange();
            UpdateState();
            UpdateMovement();
        } else if (Input.GetKeyDown(ACTIVATE)) {
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
    }

    private void UpdateState() {
        if (currentState == ControlState.HAND_LEFT || currentState == ControlState.HAND_RIGHT) {
            UpdateHands();
        } else if (currentState == ControlState.CAMERA) {
            UpdateCamera();
        }
    }

    private void UpdateHands() {
        if (JustPressed(HAND_GRAB)) {
            PassGrabInput();
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
            throw new NotImplementedException("ControlState not implemented: " + state);
        }
        currentState = state;
    }

    private void PassGrabInput() {
        VRInput.ControlDown(Controls.Grab, GetHandType());
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
            throw new NotImplementedException("ControlState not implemented: " + currentState); 
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
