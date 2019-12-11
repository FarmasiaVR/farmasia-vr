using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASD : MonoBehaviour {

    Rigidbody[] rbs;

    // Start is called before the first frame update
    void Start() {
        rbs = GameObject.FindObjectsOfType<Rigidbody>();

        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = true;
        }

        StartCoroutine(Shake());
    }

    private IEnumerator Shake() {

        while(true) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                break;
            }
            yield return null;
        }

        foreach (var rb in rbs) {
            rb.isKinematic = false;
        }

        float amount = 0.8f;

        for (int i = 0; i < 4; i++) { 

            AddShake(amount);

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.2f);
        AddShake(0.3f);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4; i++) {

            AddShake(amount);

            yield return new WaitForSeconds(0.45f);

            amount *= 1.1f;
        }

        yield return new WaitForSeconds(0.78f);
        AddShake(2);
    }

    private void AddShake(float amount) {

        foreach (Rigidbody rb in rbs) {
            rb.velocity = Shake(amount);
            rb.angularVelocity = Shake(amount);
        }
    }
    private Vector3 Shake(float amount) {

        Vector3 speed = new Vector3(Random.value * amount, 2*Random.value * amount -1, 2*Random.value * amount -1);

        return speed;
    }
}
