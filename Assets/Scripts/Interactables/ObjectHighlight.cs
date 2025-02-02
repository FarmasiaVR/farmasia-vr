using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour {

    #region fields
    private List<Material> materials;
    private Color highlightColor;
    private Color openingSpotHighlightColor;
    private Color normalColor;

    private bool highlighted;
    private bool disableHighlighting;
    #endregion

    private void Awake() {
        InitializeMaterials();
        highlightColor = new Color32(100, 120, 100, 1);
        openingSpotHighlightColor = new Color32(255, 0, 0, 1);
        normalColor = new Color32(0, 0, 0, 0);
    }

    private void OnDestroy() {
        Unhighlight();
    }

    public void Highlight() {
        if (disableHighlighting || highlighted) {
            return;
        }
        highlighted = true;
        if (this.transform.gameObject.name == "Right opening spot" || this.transform.gameObject.name == "Wrong opening spot"
            || this.transform.gameObject.name == "Push") {
            for (int i = 0; i < materials.Count; i++) {
                materials[i].SetColor("_EmissionColor", openingSpotHighlightColor);
            }
        }
        else {
            for (int i = 0; i < materials.Count; i++) {
                materials[i].SetColor("_EmissionColor", highlightColor);
            }
        }
    }

    public void Unhighlight() {
        if (disableHighlighting || !highlighted) {
            return;
        }
        highlighted = false;
        for (int i = 0; i < materials.Count; i++) {
            materials[i].SetColor("_EmissionColor", normalColor);
        }
    }

    private void InitializeMaterials() {
        materials = new List<Material>();

        Renderer r = GetComponent<Renderer>();

        AddMaterialsFromRenderer(r);

        foreach (Renderer child in transform.GetComponentsInChildren<Renderer>()) {
            AddMaterialsFromRenderer(child);
        }
    }

    public void DisableHighlighting(bool disableHighlighting) {
        this.disableHighlighting = disableHighlighting;
    }

    private void AddMaterialsFromRenderer(Renderer r) {
        if (r != null) {
            for (int i = 0; i < r.materials.Length; i++) {
                r.materials[i].EnableKeyword("_EMISSION");
                materials.Add(r.materials[i]);
            }
        }
    }
}