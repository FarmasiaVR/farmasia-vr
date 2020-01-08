using UnityEngine;

public class MenuInterface : MonoBehaviour {

    [SerializeField]
    private GameObject menuContainer;
    private Hand hand;
    [SerializeField]
    private Transform camera;
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float lerpAmount;

    public bool Visible => menuContainer.activeSelf;

    public void Close() {
        menuContainer.SetActive(!Visible);
    }

    private void Start() {
        if (camera == null) {
            camera = GameObject.FindGameObjectWithTag("MainCamera")?.transform;
        }
        hand = GameObject.FindGameObjectWithTag("Controller (Left)")?.GetComponent<Hand>();
    }

    private void Update() {
        if (Visible) {
            transform.LookAt(camera, Vector3.up);
            transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), Time.deltaTime / lerpAmount);
        }
        if (hand != null && VRInput.GetControlDown(hand.HandType, Controls.Menu)) {
            Close();
        } else if (hand == null) {
            Logger.Warning("Hand is Null!");
        }
    }

    private Vector3 GetTargetPosition() {
        return camera.TransformPoint(offset);
    }
}
