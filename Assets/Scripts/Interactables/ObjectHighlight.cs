using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour {

    #region fields
    private List<Material> materials;
    private Color highlightColor;
    private Color normalColor;

    #endregion

    private void Awake() {
        InitializeLists();
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

    private void InitializeLists() {
        materials = new List<Material>();

        Renderer r = GetComponent<Renderer>();

        if (r != null && r.material != null) {
            materials.Add(r.material);
        }

        foreach (Renderer rc in transform.GetComponentsInChildren<Renderer>()) {
            if (rc.material != null) {
                materials.Add(rc.material);
            }
        }
    }
}
