using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildConnection : ItemConnection {

    protected override ItemConnector Connector { get; set; }
    private Transform target;
    private Interactable interactable;

    private void Awake() {
        interactable.RigidbodyContainer.Disable();
        SafeSetParent(target, transform);
    }

    public static ChildConnection Configuration(ItemConnector connector, Transform target, GameObject addTo) {

        ChildConnection conn = addTo.AddComponent<ChildConnection>();

        conn.Connector = connector;
        conn.target = target;
        conn.interactable = addTo.GetComponent<Interactable>();

        return conn;
    }

    private void OnDestroy() {
        interactable.RigidbodyContainer.EnableAndDeparent();
    }

    public static void SafeSetParent(Transform parent, Transform child) {
        child.parent = child;
    }
}
