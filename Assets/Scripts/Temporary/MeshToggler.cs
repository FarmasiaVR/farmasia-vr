using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshToggler : MonoBehaviour {

    private Valve.VR.SteamVR_Input_Sources type;

    void Start() {
        type = GetComponent<VRHandControls>().handType;
    }

    void Update() {
        if (VRInput.GetControlDown(type, Controls.Grab)) {
            SetRenderers(transform, false);
        } else if (VRInput.GetControlUp(type, Controls.Grab)) {
            SetRenderers(transform, true);
        }
    }

    private void SetRenderers(Transform t, bool enable) {

        Renderer r = t.GetComponent<Renderer>();

        if (r != null) {
            r.enabled = enable;
        }

        foreach (Transform c in t) {
            SetRenderers(c, enable);
        }
    }
}
