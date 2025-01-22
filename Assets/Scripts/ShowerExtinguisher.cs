using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerExtinguisher : MonoBehaviour {

    private ITogglableFire fireSource;
    private PlayerInteractions playerHitbox;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "FireGrid") {
            ITogglableFire fire = other.GetComponentInParent(typeof(ITogglableFire)) as ITogglableFire;
            if (fire != null)
                fireSource = fire;
        } else if (other.tag == "PlayerCollider") {
            playerHitbox = other.GetComponent<PlayerInteractions>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "FireGrid") {
            fireSource = null;
        } else if (other.tag == "PlayerCollider") {
            playerHitbox = null;
        }
    }

    public void Extinguish() {
        if (fireSource != null)
            fireSource.Extinguish();
        
        if (playerHitbox != null)
            playerHitbox.Shower();
    }
}
