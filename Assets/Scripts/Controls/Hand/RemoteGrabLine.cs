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
        Transform line = transform;
        Vector3 forward = transform.parent.forward;
        Vector3 toHead = cam.position - line.position;

        // Look at player
        line.forward = -toHead;
        // Rotate line to match controller direction on a 2D level
        line.Rotate(toHead, Vector3.SignedAngle(line.up, Vector3.ProjectOnPlane(forward, toHead), toHead), Space.World);
        // Tilt line to add depth and match the real direction of the controller
        Vector3 pivot = Vector3.Cross(forward, line.up);
        line.Rotate(pivot, Vector3.SignedAngle(line.up, forward, pivot), Space.World);
    }

    public void Enable(bool enable) {
        if (isEnabled == enable) {
            return;
        }
        isEnabled = enable;
        meshRenderer.enabled = enable;
    }
}
