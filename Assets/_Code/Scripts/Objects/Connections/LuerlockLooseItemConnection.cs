﻿using System;
using UnityEngine;

public class LuerlockLooseItemConnection : ItemConnection {

    #region Fields
    private static float luerlockBreakDistance = 0.05f;
    private static float luerlockItemDistance = 0.15f;

    protected override ItemConnector Connector { get; set; }
    private Interactable interactable;
    private GeneralItem parentItem;
    private Hand hand;

    private Vector3 startLocal;

    private InteractState state;

    private Vector3 TargetPos {
        get {
            return parentItem.transform.TransformPoint(startLocal);
        }
    }
    #endregion

    // TODO: Can this be done in a coroutine?
    protected void Update() {
        UpdatePosition();
    }

    private void UpdatePosition() {

        Vector3 newPos = PosInBetween(luerlockItemDistance);

        float distance = Vector3.Distance(TargetPos, newPos);

        if (distance > luerlockBreakDistance * luerlockItemDistance) {
            BreakLuerlockConnection();
            return;
        }

        transform.position = newPos;
    }

    private Vector3 PosInBetween(float factor) {

        float distance = Vector3.Distance(TargetPos, hand.Smooth.transform.position);
        Vector3 direction = (hand.Smooth.transform.position - TargetPos).normalized;

        return TargetPos + direction * distance * factor;
    }

    protected override void OnRemoveConnection() {
        transform.localPosition = startLocal;
    }

    private void BreakLuerlockConnection() {

        // Move into ConnectionHandler?

        Remove();

        if (state == InteractState.LuerlockAttached) {
            (parentItem as LuerlockAdapter).GetConnector(interactable).Connection.Remove();
        } else if (state == InteractState.ConnectableAttached) {
            (parentItem as ConnectableItem).Connector.Connection.Remove();
        }

        interactable.transform.position = hand.Smooth.transform.position;
        interactable.transform.rotation = hand.Smooth.transform.rotation;

        hand.InteractWith(interactable, false);
        hand.Smooth.DisableInitMode();
    }

    public static LuerlockLooseItemConnection Configuration(ItemConnector connector, Transform hand, Interactable interactable) {

        if (interactable.State == InteractState.LuerlockAttached) {
            return LuerlockConfiguration(connector, hand, interactable);
        } else if (interactable.State == InteractState.ConnectableAttached) {
            return ConnectableConfiguration(connector, hand, interactable);
        }

        throw new Exception("No such configuration type for InteractState");
    }

    public static LuerlockLooseItemConnection LuerlockConfiguration(ItemConnector connector, Transform hand, Interactable interactable) {

        LuerlockLooseItemConnection conn = interactable.gameObject.AddComponent<LuerlockLooseItemConnection>();

        conn.Connector = connector;
        conn.interactable = interactable;
        conn.hand = hand.GetComponent<Hand>();
        conn.parentItem = interactable.Interactors.LuerlockPair.Value;
        conn.startLocal = interactable.transform.localPosition;
        conn.state = InteractState.LuerlockAttached;

        return conn;
    }
    public static LuerlockLooseItemConnection ConnectableConfiguration(ItemConnector connector, Transform hand, Interactable interactable) {
        LuerlockLooseItemConnection conn = interactable.gameObject.AddComponent<LuerlockLooseItemConnection>();

        conn.Connector = connector;
        conn.interactable = interactable;
        conn.hand = hand.GetComponent<Hand>();
        conn.parentItem = interactable.Interactors.ConnectableItem;
        conn.startLocal = interactable.transform.localPosition;
        conn.state = InteractState.ConnectableAttached;

        return conn;
    }
}
