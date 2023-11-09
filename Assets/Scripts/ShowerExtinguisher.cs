using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerExtinguisher : MonoBehaviour {

    private ITogglableFire fireSource;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "FireGrid" || other.tag == "PlayerCollider") {
            ITogglableFire fire = other.GetComponentInParent(typeof(ITogglableFire)) as ITogglableFire;
            if (fire != null)
                fireSource = fire;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "FireGrid" || other.tag == "PlayerCollider") {
            fireSource = null;
        }
    }

    public void Extinguish() {
        if (fireSource != null) {
            fireSource.Extinguish();
        }
    }
}
