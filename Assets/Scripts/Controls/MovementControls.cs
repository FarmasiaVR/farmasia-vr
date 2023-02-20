using UnityEngine;
using Valve.VR;

public class MovementControls : MonoBehaviour {

    private SteamVR_Action_Boolean menuAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Menu");
    private Hand rightHand;


    [SerializeField]
    private Transform player;
    private Bounds[] bounds;
    private float movementStep = 0.05f;

    private void Start() {
        rightHand = GetComponent<Hand>();
        player = Player.Transform;
        GetPlayAreas();
    }

    private void Update() {
        if ((menuAction != null && menuAction.GetStateDown(rightHand.HandType)) || VRInput.GetControlDown(SteamVR_Input_Sources.RightHand, ControlType.Menu)) {
            Move();
        }
    }

    private void GetPlayAreas() {
        GameObject playAreas = GameObject.FindGameObjectWithTag("PlayArea");
        if (playAreas == null || playAreas.transform.childCount == 0) {
            Logger.Warning("Play areas missing");
            return;
        }
        bounds = new Bounds[playAreas.transform.childCount];
        for (int i = 0; i < bounds.Length; i++) {
            bounds[i] = playAreas.transform.GetChild(i).GetComponent<Collider>().bounds;
        }
        Destroy(playAreas);
    }

    private void Move() {
        Vector3 newPos = player.position + GetPointedDirection() * movementStep;
        if (ValidPosition(newPos)) {
            player.position = newPos;
        }
    }

    private bool ValidPosition(Vector3 pos) {
        if (bounds == null) {
            return true;
        }
        foreach (Bounds b in bounds) {
            if (b.Contains(pos)) {
                return true;
            }
        }
        return false;
    }

    private Vector3 GetPointedDirection() {
        Vector3 forward = transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
}
