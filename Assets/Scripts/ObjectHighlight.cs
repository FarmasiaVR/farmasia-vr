using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour {
    private Material material;
    private Color startcolor;

    private bool highlighted;

    void Start() {
        material = GetComponent<Renderer>().material;
        startcolor = material.color;
    }

    private void OnDestroy() {
        Unhighlight();
    }

    public void Highlight() {
        startcolor = material.color;
        material.color = Color.yellow;
        //material.color = material.color * 1.2f;
    }

    public void Unhighlight() {
        material.color = startcolor;
    }

    public IEnumerator InsideCheck(HandCollider coll) {
        
        while (coll.Contains(gameObject)) {

            bool closest = gameObject == coll.GetGrabObject();
            Logger.PrintVariables("closest", closest);
            if (closest && !highlighted) {
                Highlight();
                highlighted = true;
            } else if (!closest && highlighted) {
                Unhighlight();
                highlighted = false;
            }


            yield return null;
        }

        Unhighlight();
    }
}
