using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour {

    #region fields
    private List<Material> materials;
    private Color highlightColor;
    private Color normalColor;

    #endregion

    private void Awake() {
        InitializeMaterials();
        highlightColor = new Color32(100, 120, 100, 1);
        normalColor = new Color32(0, 0, 0, 0);
    }

    private void OnDestroy() {
        Unhighlight();
    }

    public void Highlight() {
        for (int i = 0; i < materials.Count; i++) {
            materials[i].SetColor("_EmissionColor", highlightColor);
        }
    }

    public void Unhighlight() {
        for (int i = 0; i < materials.Count; i++) {
            materials[i].SetColor("_EmissionColor", normalColor);
        }
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
                r.materials[i].EnableKeyword("_EMISSION");
                materials.Add(r.materials[i]);
            }
        }
    }
}