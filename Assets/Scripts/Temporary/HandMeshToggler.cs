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
                if (transform.name.Equals("body")) {
                    if (hand.tag.Equals("Controller (Left)")) transform.gameObject.layer = 14;
                    else if (hand.tag.Equals("Controller (Right)")) transform.gameObject.layer = 15;
                    MeshCollider meshCollider = transform.gameObject.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                    meshCollider.isTrigger = true;
                }

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
