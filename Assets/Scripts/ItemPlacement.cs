using System.Collections;
using UnityEngine;

// Replace
public class ItemPlacement : MonoBehaviour {

    private Rigidbody rb;
    private bool collided;

    private static float maxDistance = 1;
    private static float safeDropHeight = 0.2f;
    private static float slowDownTime = 1;
    private static float slowFactor = 0.7f;

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();

        if (rb == null) {
            throw new System.Exception("no rigidbody");
        }

        // StartSlowDown();
    }

    public void CancelItemPlacement() {
        StopAllCoroutines();
    }
    private void StartSlowDown() {
        StartCoroutine(SlowDown());
    }

    private void OnCollisionEnter(Collision collision) {
        if (collided) return;
        collided = true;
        StartCoroutine(SlowDown());
    }

    private IEnumerator SlowDown() {

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        float time = slowDownTime;

        while (time > 0) {

            // Framerate dependent, fix pls
            Vector3 vel = rb.velocity;
            vel *= slowFactor;

            rb.velocity = vel;

            time -= Time.deltaTime;
            yield return null;
        }

        this.enabled = false;
    }


    // Replace
    public static void ReleaseSafely(GameObject g) {
        if (g.GetComponent<ItemPlacement>() == null) return;
        if (g.GetComponent<Interactable>().State != InteractState.Grabbed) return;

        float boundsDistance = Vector3.Distance(g.GetComponentInChildren<Collider>().bounds.ClosestPoint(g.transform.position - Vector3.down * maxDistance), g.transform.position);
        float rayLength = safeDropHeight + boundsDistance;

        RaycastHit hit;
        if (!Physics.Raycast(g.transform.position, Vector3.down, out hit, rayLength)) return;
        if (hit.transform.tag == "Interactable") return;

        ItemPlacement p = g.GetComponent<ItemPlacement>();
        p.enabled = true;
        p.collided = false;
        p.StartSlowDown();
    }
}
