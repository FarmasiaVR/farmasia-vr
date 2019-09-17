using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacement : MonoBehaviour {

    private Rigidbody rb;

    private static float maxDistance = 1;
    private static float safeDropHeight = 0.2f;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update() {

    }

    public void TryPlacement(Transform item) {
        float angle = Vector3.Angle(item.up, Vector3.up);
        if (angle > 25f) {

        }
    }

    private void OnCollisionEnter(Collision collision) {
        rb.velocity = Vector3.zero;

        Destroy(this);
    }

    public static void ReleaseSafely(GameObject g) {

        float boundsDistance = Vector3.Distance(g.GetComponent<Collider>().bounds.ClosestPoint(g.transform.position - Vector3.down * maxDistance), g.transform.position);

        float rayLength = safeDropHeight + boundsDistance;

        RaycastHit hit;
        if (!Physics.Raycast(g.transform.position, Vector3.down, out hit, rayLength)) {
            return;
        }

        g.AddComponent<ItemPlacement>();
    }
}
