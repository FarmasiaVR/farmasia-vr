using UnityEngine;

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

    private Hand right, left;
    private Hand current;
    private bool isGrabbing;
    #endregion

    void Start() {
        right = transform.GetChild(0).GetComponent<Hand>();
        left = transform.GetChild(1).GetComponent<Hand>();
        current = left;
    }

    void Update() {
        UpdateHands();
        UpdateMovement();
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

        current.transform.Translate(movement * handSpeed * Time.deltaTime);
    }

    private void ActivateLeft() {
        if (current != left) {
            current = left;
            return;
        }
        GrabObject();
    }

    private void ActivateRight() {
        if (current != right) {
            current = right;
            return;
        }
        GrabObject();
    }

    private void GrabObject() {
        current.InteractWithObject();
    }

    private void ReleaseLeftObject() {
        if (current == left) {
            current.UninteractWithObject();
        }
    }

    private void ReleaseRightObject() {
        if (current == right) {
            current.UninteractWithObject();
        }
    }

    private void Interact() {
        current?.GrabInteract();
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
