using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour {

    #region fields
    private List<Material> materials;
    //private List<Color> startColors;
    private Color highlightColor;
    private Color normalColor;

    #endregion

    private void Start() {
        InitializeLists();
        highlightColor = new Color32(60,60,60,100);
        normalColor = new Color32(0,0,0,0);
    }

    private void OnDestroy() {
        Unhighlight();
    }

    public void Highlight() {
        for (int i = 0; i < materials.Count; i++) {
            materials[i].SetColor("_EMISSION", highlightColor);
        }
    }

    public void Unhighlight() {
        for (int i = 0; i < materials.Count; i++) {
            materials[i].SetColor("_EMISSION", normalColor);
        }
    }

    public IEnumerator InsideCheck(HandCollider coll) {
        while (coll.Contains(gameObject)) {
            bool isClosest = gameObject == coll.GetGrabObject();

            if (gameObject.GetComponent<Interactable>().State == InteractState.Grabbed) {
                Unhighlight();
            } else if (isClosest) {
                Highlight();
            } else if (!isClosest) {
                Unhighlight();
            }

            yield return null;
        }

        Unhighlight();
    }

    private void InitializeLists() {
        List<Material> m = new List<Material>();
        //List<Color> c = new List<Color>();

        AddObjectMaterial(transform);

        foreach (Transform t in transform) {
            AddObjectMaterial(t);
        }

        materials = m;
        //startColors = c;


        void AddObjectMaterial(Transform tt) {
            Renderer r = tt.GetComponent<Renderer>();
            if (r != null) {
                r.material.EnableKeyword("_EMISSION");
                m.Add(r.material);
                //c.Add(r.material.color);
            }
        }
    }
}
