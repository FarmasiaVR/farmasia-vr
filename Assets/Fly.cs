using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Fly : MonoBehaviour {
    public Rigidbody rb;
    private int selectExitCount = 0;
    private float timer = 0.0f;

    public void startFlying() {
        selectExitCount += 1;
    }

    // Update is called once per frame
    void Update() {
        if (selectExitCount >= 2)
            timer += Time.deltaTime;

        if (timer >= 5.0f) {
            if (rb.useGravity)
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;

            Vector3 newPos = transform.position; 
            newPos.y += 4 * Time.deltaTime;
            transform.position = newPos;
        }
    }
}
