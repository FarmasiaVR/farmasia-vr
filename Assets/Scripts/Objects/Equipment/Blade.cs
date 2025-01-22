using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

    private Cover cover;

    public bool cutXR;

    void Start() {
        cover = transform.parent.GetComponentInChildren<Cover>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!enabled) return;
        if (other.isTrigger) return;
        var filter = other.gameObject.transform.parent?.GetComponent<PumpFilterFilter>();
        if (cover.CoverOn || filter == null) return;
        if (filter.CanBeCut) { 
            if (cutXR)
            {
                filter.cutXR(transform);
            } else
            {
                filter.Cut(transform);
            }    
        }
    }


}
