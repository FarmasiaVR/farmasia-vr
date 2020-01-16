using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteGrabLine : MonoBehaviour {

    #region fields
    private MeshRenderer meshRenderer;
    private Material material;
    private bool isEnabled;

    private const float SPEED = -0.3f;
    #endregion

    private void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        material = meshRenderer.material;
    }

    private void Update() {
        if (!isEnabled) {
            return;
        }

        float newY = material.mainTextureOffset.y + Time.deltaTime * SPEED;
        if (newY > 1) {
            newY -= 1;
        }
        material.mainTextureOffset = new Vector2(0, newY);
    }

    public void Enable(bool enable) {
        if (isEnabled == enable) {
            return;
        }
        isEnabled = enable;
        meshRenderer.enabled = enable;
    }
}
