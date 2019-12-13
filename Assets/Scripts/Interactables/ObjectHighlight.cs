using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour {

    #region fields
    private List<Material> materials;
    private Color32 highlightColor;
    private Color32[] normalColor;
    #endregion

    private void Awake() {
        InitializeMaterials();
        highlightColor = new Color32(150, 150, 150, 0);
    }

    private void OnDestroy() {
        Unhighlight();
    }

    public void Highlight() {
        normalColor = new Color32[materials.Count];
        for (int i = 0; i < materials.Count; i++) {
            if (materials[i].HasProperty("_Color")) {
                normalColor[i] = materials[i].color;
                materials[i].color += highlightColor;
            }
        }
    }

    public void Unhighlight() {
        if (normalColor == null) {
            return;
        }

        for (int i = 0; i < materials.Count; i++) {
            if (materials[i].HasProperty("_Color")) {
                materials[i].color = normalColor[i];
            }
        }
        normalColor = null;
    }

    public static ObjectHighlight GetHighlightFromTransform(Transform t) {
        return Interactable.GetInteractableObject(t).GetComponent<ObjectHighlight>();
    }

    private void InitializeMaterials() {
        materials = new List<Material>();

        Renderer r = GetComponent<Renderer>();

        AddMaterialsFromRenderer(r);

        foreach (Renderer child in transform.GetComponentsInChildren<Renderer>()) {
            AddMaterialsFromRenderer(child);
        }
    }

    private void AddMaterialsFromRenderer(Renderer r) {
        if (r != null) {
            for (int i = 0; i < r.materials.Length; i++) {
                materials.Add(r.materials[i]);
            }
        }
    }
}