using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildConnection : ItemConnection {

    protected override ItemConnector Connector { get; set; }
    private Transform target;
    private Interactable interactable;

    private void Init() {
        interactable = GetComponent<Interactable>();
        interactable.RigidbodyContainer.Disable();
        SafeSetParent(target, transform);
    }

   
    protected override void RemoveConnection() {
        interactable.RigidbodyContainer.EnableAndDeparent();
    }

    public static void SafeSetParent(Transform parent, Transform child) {
        Logger.PrintVariables("set", "parent", "parent", parent, "child", child);
        child.SetParent(parent);
    }

    public static ChildConnection Configuration(ItemConnector connector, Transform target, Interactable addTo) {

        ChildConnection conn = addTo.gameObject.AddComponent<ChildConnection>();

        conn.Connector = connector;
        conn.target = target.transform;

        conn.Init();

        return conn;
    }
}
