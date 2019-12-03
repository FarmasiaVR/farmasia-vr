using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMeshToggler : MonoBehaviour {

    private Renderer[] renderers;
    private Hand hand;
    private bool status;

    void Start() {
        hand = GetComponent<Hand>();
        status = enabled;

        StartCoroutine(FindRenderersLate());

        IEnumerator FindRenderersLate() {

            while (transform.Find("Model").childCount == 0) {
                yield return null;
            }

            renderers = GetComponentsInChildren<Renderer>();
        }
    }

    private void Update() {
        UpdateMesh();
    }

    private void UpdateMesh() {
        if (hand.IsGrabbed) {
            Show(false);
        } else {
            Show(true);
        }
    }

    private void Show(bool hide) {
        if (status == hide) {
            return;
        }

        status = hide;
        SetRenderers();
    }

    private void SetRenderers() {
#if UNITY_VRCOMPUTER
        foreach (Renderer r in renderers) {
            if (r != null) {
                r.enabled = status;
            }
        }
#endif
    }

}
