using System;
using UnityEngine;

public class LuerlockLooseItemConnection : ItemConnection {

    #region Fields
    private static float luerlockBreakDistance = 0.045f;

    protected override ItemConnector Connector { get; set; }
    private Interactable interactable;
    private LuerlockAdapter luerlock;
    private Hand hand;

    private Vector3 startLocal;

    private Vector3 TargetPos {
        get {
            return luerlock.transform.TransformPoint(startLocal);
        }
    }
    #endregion

    protected override void Update() {
        UpdatePosition();
    }

    private void UpdatePosition() {

        Vector3 newPos = PosInBetween(0.15f);

        float distance = Vector3.Distance(TargetPos, newPos);

        if (distance > luerlockBreakDistance) {
            BreakLuerlockConnection();
            return;
        }

        transform.position = newPos;
    }

    private Vector3 PosInBetween(float factor) {

        float distance = Vector3.Distance(TargetPos, hand.transform.position);
        Vector3 direction = (hand.transform.position - TargetPos).normalized;

        return TargetPos + direction * distance * factor;
    }

    protected override void OnRemoveConnection() {
        transform.localPosition = startLocal;
    }

    private void BreakLuerlockConnection() {

        // Move into ConnectionHandler?

        Remove();
        luerlock.GetConnector(interactable).Connection.Remove();

        interactable.transform.position = hand.transform.position;

        hand.InteractWith(interactable);
    }

    public static LuerlockLooseItemConnection Configuration(ItemConnector connector, Transform hand, Interactable interactable) {

        LuerlockLooseItemConnection conn = interactable.gameObject.AddComponent<LuerlockLooseItemConnection>();

        conn.Connector = connector;
        conn.interactable = interactable;
        conn.hand = hand.GetComponent<Hand>();
        conn.luerlock = interactable.Interactors.LuerlockPair.Value;
        conn.startLocal = interactable.transform.localPosition;

        return conn;
    }
}
