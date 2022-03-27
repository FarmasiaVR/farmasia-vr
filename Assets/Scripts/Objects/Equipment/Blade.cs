using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

    private Cover cover;

    void Start() {
        cover = transform.parent.GetComponentInChildren<Cover>();
    }

    void onTriggerEnter(Collider collider) {
        Logger.Print("Watafaaak");
        var filter = collider.gameObject.GetComponent<PumpFilterFilter>();
        Logger.Print("Blade hit " + filter + " and cover is " + cover.CoverOn);
        if (cover.CoverOn) return;
        if (filter == null) return;
        Logger.Print("Cutting Filter");
    }
}
