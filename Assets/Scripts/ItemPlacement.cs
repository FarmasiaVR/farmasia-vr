using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacement : MonoBehaviour {

    private Rigidbody rb;

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
}
