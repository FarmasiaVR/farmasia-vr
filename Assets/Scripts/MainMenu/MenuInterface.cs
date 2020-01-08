using UnityEngine;

public class MenuInterface : MonoBehaviour {

    [SerializeField]
    private GameObject menuContainer;
    private Hand hand;
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float lerpAmount;
    [SerializeField]
    private float centerOffset = 0.2f;
    private Vector3 CameraCenter { get => cam.position + cam.forward * centerOffset; }

    public bool Visible => menuContainer.activeSelf;

    public void Close() {
        menuContainer.SetActive(!Visible);
    }

    private void Start() {
        if (cam == null) {
            cam = GameObject.FindGameObjectWithTag("MainCamera")?.transform;
        }
        hand = GameObject.FindGameObjectWithTag("Controller (Left)")?.GetComponent<Hand>();
    }

    private void Update() {
        if (Visible) {
            transform.LookAt(cam, Vector3.up);
            transform.position = Vector3.Lerp(transform.position, GetTransformPosition(), Time.deltaTime / lerpAmount);
        }
        if (hand != null && VRInput.GetControlDown(hand.HandType, Controls.Menu)) {
            Close();
        } else if (hand == null) {
            Logger.Warning("Hand is Null!");
        }
    }

    private Vector3 GetTransformPosition() {
        Vector3 forward = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;
        Vector3 right = new Vector3(cam.right.x, 0, cam.right.z).normalized;

        Vector3 targetPosition = CameraCenter + forward * offset.z + right * offset.x;
        targetPosition = new Vector3(targetPosition.x, CameraCenter.y + offset.y, targetPosition.z);
        return targetPosition;
    }
}
