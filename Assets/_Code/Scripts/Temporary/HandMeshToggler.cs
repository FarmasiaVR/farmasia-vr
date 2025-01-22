using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMeshToggler : MonoBehaviour {

    private Hand hand;
    private List<Renderer> renderers;

    public bool Status { get; set; }

    void Start() {
        hand = GetComponent<Hand>();
        renderers = new List<Renderer>();
        Status = enabled;

        IEnumerator FindRenderersLate() {
            // Keep looping until controller is turned on
            while (transform.Find("Model").childCount == 0) {
                yield return null;
            }

            SearchRenderers(transform);
        }

        StartCoroutine(FindRenderersLate());
    }

    private void SearchRenderers(Transform transform) {
        if (transform.GetComponent<RemoteGrabLine>() == null) {
            if (transform.GetComponent<Renderer>() is var renderer && renderer != null) {
                renderers.Add(renderer);
            }
        }

        foreach (Transform child in transform) {
            SearchRenderers(child);
        }
    }

    private void Update() {
        if (hand.IsGrabbed) {
            Show(false);
        } else {
            Show(true);
        }
    }

    public void Show(bool hide) {
        if (Status == hide) {
            return;
        }

        Status = hide;
        SetRenderers();
    }

    private void SetRenderers() {
        #if UNITY_VRCOMPUTER
        foreach (Renderer renderer in renderers) {
            if (renderer != null) {
                renderer.enabled = Status;
            }
        }
        #endif
    }
}
