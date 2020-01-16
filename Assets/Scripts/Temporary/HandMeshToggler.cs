using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMeshToggler : MonoBehaviour {

    private Renderer[] renderers;
    private Hand hand;
    public bool Status { get; private set; }

    void Start() {
        hand = GetComponent<Hand>();
        Status = enabled;

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

    public void Show(bool hide) {
        if (Status == hide) {
            return;
        }

        Status = hide;

        SetRenderers();
    }

    private void SetRenderers() {
#if UNITY_VRCOMPUTER
        foreach (Renderer r in renderers) {
            if (r != null) {
                r.enabled = Status;
            }
        }
#endif
    }

}
