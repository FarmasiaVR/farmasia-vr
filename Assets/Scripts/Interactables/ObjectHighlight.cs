using System.Collections;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour {

    #region fields
    private Material material;
    private Color startColor;
    private Color highlightColor;

    #endregion

    private void Start() {
        material = GetComponent<Renderer>().material;
        startColor = material.color;
        highlightColor = startColor + new Color32(40,40,40,0);
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

            if (coll.Hand.IsGrabbed) {
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
}
