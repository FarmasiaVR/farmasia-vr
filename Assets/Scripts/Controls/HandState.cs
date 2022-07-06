using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandState : MonoBehaviour {

    public Renderer rend;

    public void SetDirty() {
        rend.material.SetInt("_BlendOpacity", 10);
        rend.material.SetInt("_Dirty", 1);
    }

    public void SetSanitized() {
        rend.material.SetInt("_Dirty", 0);
    }

    public void SetClean() {
        rend.material.SetInt("_BlendOpacity", 0);
    }
}
