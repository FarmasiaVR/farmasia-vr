using UnityEngine;

public class TestHandMover : MonoBehaviour {

    private bool usingRight;

    Vector3 movement;

    private float handSpeed = 1;

    private Hand right, left;

    void Start() {
        right = transform.GetChild(0).GetComponent<Hand>();
        left = transform.GetChild(1).GetComponent<Hand>();
    }

    void Update() {
        UpdateInteract();
        UpdateHandStatus();
        CheckInput();
        MoveHand();
    }

    private void UpdateInteract() {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !usingRight) {
            left.InteractWithObject();
        } else if (Input.GetKeyDown(KeyCode.Mouse1) && usingRight) {
            right.InteractWithObject();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && !usingRight) {
            left.UninteractWithObject();
        } else if (Input.GetKeyUp(KeyCode.Mouse1) && usingRight) {
            right.UninteractWithObject();
        }

        if (Input.GetKeyDown(KeyCode.Mouse2) && !usingRight) {
            left.GrabInteract();
        } else if (Input.GetKeyUp(KeyCode.Mouse2) && usingRight) {
            right.GrabInteract();
        }
    }

    private void UpdateHandStatus() {
        if (Input.GetKeyDown(KeyCode.Mouse0) && usingRight) {
            usingRight = false;
        } else if (Input.GetKeyDown(KeyCode.Mouse1) && !usingRight) {
            usingRight = true;
        }
    }

    private void CheckInput() {
        movement = Vector3.zero;

        if (Pressing(KeyCode.W)) {
            movement.z++;
        }
        if (Pressing(KeyCode.S)) {
            movement.z--;
        }

        if (Pressing(KeyCode.D)) {
            movement.x++;
        }
        if (Pressing(KeyCode.A)) {
            movement.x--;
        }

        if (Pressing(KeyCode.E)) {
            movement.y++;
        }
        if (Pressing(KeyCode.Q)) {
            movement.y--;
        }
    }

    private void MoveHand() {
        Transform hand = usingRight ? right.transform : left.transform;
        hand.Translate(movement * handSpeed * Time.deltaTime);
    }

    private bool Pressing(KeyCode c) {
        return Input.GetKey(c);
    }
}
