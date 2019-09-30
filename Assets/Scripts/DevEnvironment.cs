using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevEnvironment : MonoBehaviour {

    [SerializeField]
    private bool show;
    private Valve.VR.SteamVR_Input_Sources type = Valve.VR.SteamVR_Input_Sources.LeftHand;

    private void Start() {
        RecursiveChildSearch(transform);
    }

    private void Update() {
        
        if (VRInput.GetControlDown(type, Controls.DevEnv)) {
            show = !show;
            RecursiveChildSearch(transform);
        }
    }

    private void RecursiveChildSearch(Transform t) {

        MeshRenderer r = t.GetComponent<MeshRenderer>();

        if (r != null) {
            r.enabled = show;
        }

        foreach (Transform tc in t) {
            RecursiveChildSearch(tc);
        }
    }
}
