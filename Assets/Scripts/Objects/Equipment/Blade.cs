using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

    private Cover cover;

    void Start() {
        cover = transform.parent.GetComponentInChildren<Cover>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger) return;
        var filter = other.gameObject.transform.parent?.GetComponent<PumpFilterFilter>();
        if (cover.CoverOn || filter == null) return;
        if (filter.CanBeCut) { 
            filter.Cut(transform);
        }
    }


}
