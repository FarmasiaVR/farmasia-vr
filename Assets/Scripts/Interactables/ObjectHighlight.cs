using System.Collections;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour {

    #region fields
    private Material material;
    private Color startColor;
    private Color highlightColor;

    #endregion

    private void Start() {
        material = Renderer.material;
        startColor = material.color;
        highlightColor = startColor + new Color32(60,60,60,0);
    }

    private void OnDestroy() {
        Unhighlight();
    }

    public void Highlight() {
        material.color = highlightColor;
    }

    public void Unhighlight() {
        material.color = startColor;
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

    // Fix better
    private Renderer Renderer {
        get {

            Renderer r = GetComponent<Renderer>();

            if (r != null) {
                return r;
            }

            foreach (Transform t in transform) {
                r = t.GetComponent<Renderer>();

                if (r != null) {
                    return r;
                }
            }

            throw new System.Exception("No renderer was found");
        }
    }
}
