using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandState : MonoBehaviour {

    public Renderer rend;

    public void SetDirty() {
        rend.material.SetFloat("_StepEdge", 0.05f);
        rend.material.SetInt("_Clean", 0);
    }

    public void SetClean() {
        rend.material.SetFloat("_StepEdge", 0.6f);
        rend.material.SetInt("_Clean", 1);
    }
}
