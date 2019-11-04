using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildConnection : ItemConnection {

    protected override ItemConnector Connector { get; set; }
    private Transform target;
    private Interactable interactable;

    private void Awake() {
        Logger.Print("Child connection awake");
        Logger.PrintVariables("target", target);
        interactable = GetComponent<Interactable>();
        interactable.RigidbodyContainer.Disable();
        SafeSetParent(target, transform);
    }

    public static ChildConnection Configuration(ItemConnector connector, Transform target, GameObject addTo) {

        Logger.Print("Child connection config");

        Logger.PrintVariables("Target", target, "AddTo", addTo);

        ChildConnection conn = addTo.AddComponent<ChildConnection>();

        conn.Connector = connector;
        conn.target = target;

        return conn;
    }

    protected override void OnDestroy() {
        interactable.RigidbodyContainer.EnableAndDeparent();
        Connector.OnReleaseItem();
    }

    public static void SafeSetParent(Transform parent, Transform child) {
        Logger.PrintVariables("set", "parent", "parent", parent, "child", child);
        child.SetParent(parent);
    }
}
