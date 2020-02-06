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

   
    protected override void OnRemoveConnection() {
        Logger.Print("Removing child connection: " + name);
        interactable.RigidbodyContainer.EnableAndDeparent();
        interactable.Rigidbody.velocity = Vector3.zero;
    }

    public static void SafeSetParent(Transform parent, Transform child) {
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
