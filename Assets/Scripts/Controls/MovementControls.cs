using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour {

    #region fields
    private Hand hand;
    private Transform player;

    private float movementStep = 0.05f;

    private Bounds[] bounds;
    #endregion

    private void Start() {
        hand = GetComponent<Hand>();
        player = Player.Transform;
        GetPlayArea();
    }

    private void GetPlayArea() {

        GameObject area = GameObject.FindGameObjectWithTag("PlayArea");

        if (area == null || area.transform.childCount == 0) {
            return;
        }

        bounds = new Bounds[area.transform.childCount];

        for (int i = 0; i < bounds.Length; i++) {
            bounds[i] = area.transform.GetChild(i).GetComponent<Collider>().bounds;
        }

        Destroy(area);
    }

    private void Update() {
        if (VRInput.GetControlDown(hand.HandType, Controls.Menu)) {
            Move();
        }
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
