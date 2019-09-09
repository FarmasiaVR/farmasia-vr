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
        if (highlighted) material.color = startcolor;
    }

    public IEnumerator InsideCheck(HandCollider coll) {
        
        while (TouchesHand(coll)) {

            bool closest = gameObject == coll.GetGrabObject();
            Logger.PrintVariables("closest", closest, "highlighted", highlighted);
            if (closest && !highlighted) {
                Highlight();
                highlighted = true;
            } else if (!closest && highlighted) {
                Unhighlight();
                highlighted = false;
                Logger.Print("UNHIGHLIGHT!");
            }


            yield return null;
        }

        Unhighlight();
    }

    private bool TouchesHand(HandCollider coll) {
        Logger.Print(coll.GrabObjects.Count);
        foreach (GameObject g in coll.GrabObjects) {
            Logger.PrintVariables("object", g);
            if (g == gameObject) return true;
        }
        return false;
    }
}
