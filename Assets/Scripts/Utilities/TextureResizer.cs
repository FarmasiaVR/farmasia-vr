using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureResizer : MonoBehaviour {

    #region fields
    private Material material;
    #endregion

    private void Start() {
        material = GetComponent<Renderer>().material;
        ResizeMaterial();
    }

    private void ResizeMaterial() {
        Vector3 scale = transform.lossyScale;
        material.mainTextureScale = new Vector2(scale.x, scale.z);
    }
}