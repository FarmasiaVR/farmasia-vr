using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteGrabLine : MonoBehaviour {

    #region fields
    private MeshRenderer meshRenderer;
    private Material material;
    private bool isEnabled;

    private const float SPEED = -0.3f;

    private Transform cam;
    #endregion

    private void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        material = meshRenderer.material;

        cam = Player.Camera.transform;
    }

    private void Update() {

        if (meshRenderer.enabled != isEnabled) {
            meshRenderer.enabled = isEnabled;
        }

        if (!isEnabled) {
            return;
        }

        TurnLine();

        float newY = material.mainTextureOffset.y + Time.deltaTime * SPEED;
        if (newY > 1) {
            newY -= 1;
        }
        material.mainTextureOffset = new Vector2(0, newY);
    }

    private void TurnLine() {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
        Vector3 angles = transform.eulerAngles;
        angles.y = 0;
        angles.z = 0;
        transform.eulerAngles = angles;
    }

    public void Enable(bool enable) {
        if (isEnabled == enable) {
            return;
        }
        isEnabled = enable;
        meshRenderer.enabled = enable;
    }
}
