using UnityEngine;
using Valve.VR;

public class TestHandMover : MonoBehaviour {

    #region fields
    private const KeyCode HAND_LEFT = KeyCode.Mouse0;
    private const KeyCode HAND_RIGHT = KeyCode.Mouse1;
    private const KeyCode HAND_INTERACT = KeyCode.Mouse1;
    private const KeyCode MOVE_FORWARD = KeyCode.W;
    private const KeyCode MOVE_LEFT = KeyCode.A;
    private const KeyCode MOVE_BACKWARDS = KeyCode.S;
    private const KeyCode MOVE_RIGHT = KeyCode.D;
    private const KeyCode MOVE_UP = KeyCode.E;
    private const KeyCode MOVE_DOWN = KeyCode.Q;

    private float handSpeed = 1;

    private Transform right, left;
    private bool usingRight;
    private bool isGrabbing;

    private bool active;

    private Transform Current {
        get {
            return usingRight ? right : left;
        }
    }

    private SteamVR_Input_Sources HandType {
        get {
            return usingRight ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
        }
    }

    #endregion

    private void Start() {
        right = transform.GetChild(1);
        left = transform.GetChild(0);
    }

    private void Update() {
        if (active) {
            UpdateHands();
            UpdateMovement();
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            active = true;
            Destroy(transform.GetComponent<SteamVR_PlayArea>());
            Destroy(right.GetComponent<SteamVR_Behaviour>());
            Destroy(left.GetComponent<SteamVR_Behaviour>());
        }
    }

    private void UpdateHands() {
        if (JustPressed(HAND_LEFT)) {
            ActivateLeft();
        } else if (JustPressed(HAND_RIGHT)) {
            ActivateRight();
        } else if (JustPressed(HAND_INTERACT)) {
            Interact();
        }

        if (JustReleased(HAND_LEFT)) {
            ReleaseLeftObject();
        } else if (JustReleased(HAND_RIGHT)) {
            ReleaseRightObject();
        }
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

        Current.transform.Translate(movement * handSpeed * Time.deltaTime);
    }

    private void ActivateLeft() {
        if (usingRight) {
            usingRight = false;
            return;
        }
        GrabObject();
    }

    private void ActivateRight() {
        if (!usingRight) {
            usingRight = true;
            return;
        }
        GrabObject();
    }

    private void GrabObject() {
        VRInput.ControlDown(Controls.Grab, HandType);
    }

    private void Interact() {
        VRInput.ControlDown(Controls.GrabInteract, HandType);
    }

    private void ReleaseLeftObject() {
        if (Current == left) {
            VRInput.ControlUp(Controls.Grab, SteamVR_Input_Sources.LeftHand);
        }
    }

    private void ReleaseRightObject() {
        if (Current == right) {
            VRInput.ControlUp(Controls.Grab, SteamVR_Input_Sources.RightHand);
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
