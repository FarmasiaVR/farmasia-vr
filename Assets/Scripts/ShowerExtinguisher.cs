using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerExtinguisher : MonoBehaviour {

    private ITogglableFire fireSource;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "FireGrid" || other.tag == "PlayerCollider") {
            fireSource = other.GetComponentInParent(typeof(ITogglableFire)) as ITogglableFire;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "FireGrid" || other.tag == "PlayerCollider") {
            fireSource = null;
        }
    }

    public void Extinguish() {
        Debug.Log("This debug print fixes a bug... C# is such a fun language yay!");
        if (fireSource != null && !fireSource.isBurning) {
            fireSource.Extinguish();
        }
    }
}
