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
        Transform obj = transform;
        Transform controller = transform.parent;

        obj.up = -controller.forward;
        obj.position = controller.position;

        Vector3 toHead = obj.position - cam.position;


        //Vector3 pivot = Vector3.Cross(controller.up, obj.up);
        //obj.Rotate(pivot, -Vector3.SignedAngle(controller.up, obj.up, pivot), Space.World);

        //Vector3 offset = controller.position - luerlockPos.position;
        //obj.position += controller.position;
    }

    public void Enable(bool enable) {
        if (isEnabled == enable) {
            return;
        }
        isEnabled = enable;
        meshRenderer.enabled = enable;
    }
}
