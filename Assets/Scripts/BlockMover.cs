using UnityEngine;

public class BlockMover : MonoBehaviour {

    #region fields
    [SerializeField]
    private float force = 5;

    private Vector2 movement;

    // Physics component
    private Rigidbody rb;
    #endregion

    private void Awake() {

    }

    private void Start() {
        // Get physics component
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        CheckInput();
        Move();
    }

    private void CheckInput() {
        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) {
            movement.y += 1;
        }

        if (Input.GetKey(KeyCode.S)) {
            movement.y -= 1;
        }

        if (Input.GetKey(KeyCode.A)) {
            movement.x -= 1;
        }

        if (Input.GetKey(KeyCode.D)) {
            movement.x += 1;
        }
    }

    private void Move() {
        rb.AddForce(Vector3.forward * movement.y * force);
        rb.AddForce(Vector3.right * movement.x * force);
    }
}
