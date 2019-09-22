using System.Collections;
using UnityEngine;

public class ItemPlacement : MonoBehaviour {

    private Rigidbody rb;

    private static float maxDistance = 1;
    private static float safeDropHeight = 0.2f;
    private static float slowDownTime = 1;
    private static float slowFactor = 0.7f;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    

    public void TryPlacement(Transform item) {
        float angle = Vector3.Angle(item.up, Vector3.up);
        if (angle > 25f) {

        }
    }

    private void OnCollisionEnter(Collision collision) {
        rb.velocity = Vector3.zero;

        StartCoroutine(SlowDown());
    }

    private IEnumerator SlowDown() {

        float time = slowDownTime;

        while (time > 0) {

            // Framerate dependent, fix pls
            Vector3 vel = rb.velocity;
            vel *= slowFactor;

            rb.velocity = vel;

            time -= Time.deltaTime;
            yield return null;
        }

        Destroy(this);
    }


    public static void ReleaseSafely(GameObject g) {
        if (g.GetComponent<ItemPlacement>() != null) return;
        if (g.GetComponent<Interactable>().State != InteractState.Grabbed) return;

        float boundsDistance = Vector3.Distance(g.GetComponent<Collider>().bounds.ClosestPoint(g.transform.position - Vector3.down * maxDistance), g.transform.position);
        float rayLength = safeDropHeight + boundsDistance;

        RaycastHit hit;
        if (!Physics.Raycast(g.transform.position, Vector3.down, out hit, rayLength)) return;
        if (hit.transform.tag == "Interactable") return;

        g.AddComponent<ItemPlacement>();    // Adding components every time causes lag, change approach
    }
}
