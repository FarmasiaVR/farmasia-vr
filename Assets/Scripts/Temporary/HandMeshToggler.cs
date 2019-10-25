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
            yield return null;
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
        foreach (Renderer r in renderers) {
            r.enabled = status;
        }
    }
}
