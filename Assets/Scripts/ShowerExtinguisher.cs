using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerExtinguisher : MonoBehaviour {

    private PlayerFireController playerBody;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerCollider") {
            playerBody = other.GetComponentInParent<PlayerFireController>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "FireGrid") {
            playerBody = null;
        }
    }

    public void Extinguish()
    {
        if (playerBody != null) {
            playerBody.Extinguish();
        }
    }
}
