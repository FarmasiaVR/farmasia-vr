using System.Collections;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour {

    #region fields
    private Material material;
    private Color startColor;

    public bool isHighlighted;
    #endregion

    void Start() {
        material = GetComponent<Renderer>().material;
        startColor = material.color;
    }

    private void OnDestroy() {
        Unhighlight();
    }

    public void Highlight() {
        startColor = material.color;
        material.color = material.color + new Color32(40,40,40,0);
        isHighlighted = true;
    }

    public void Unhighlight() {
        if (isHighlighted) material.color = startColor;
        isHighlighted = false;
    }

    public IEnumerator InsideCheck(HandCollider coll) {
        while (coll.Contains(gameObject)) {
            bool closest = gameObject == coll.GetGrabObject();

            if (coll.Hand.IsGrabbed) {
                if (isHighlighted) Unhighlight();
            } else if (closest && !isHighlighted) {
                Highlight();
            } else if (!closest && isHighlighted) {
                Unhighlight();
            }

            yield return null;
        }

        Unhighlight();
    }
}
