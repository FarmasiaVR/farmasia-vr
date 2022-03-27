using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

    private Cover cover;

    void Start() {
        cover = transform.parent.GetComponentInChildren<Cover>();
    }

    void OnTriggerEnter(Collider collider) {
        var filter = collider.gameObject.GetComponent<PumpFilterFilter>();
        if (cover.CoverOn || filter == null) return;
        if (filter.CanBeCut)
            filter.Cut();
    }
}
