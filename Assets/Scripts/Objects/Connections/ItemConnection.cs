using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemConnection : MonoBehaviour {

    protected abstract ItemConnector Connector { get; set; }

    public void Remove() {
        OnRemoveConnection();
        Destroy(this);
        Connector.OnReleaseItem();
    }

    protected virtual void OnRemoveConnection() { }

    protected virtual void Update() {

    }

    #region Static methods
    public static ItemConnection AddSmoothConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return SmoothConnection.Configuration(connector, target, addTo);
    }
    public static ItemConnection AddRigidConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return RigidConnection.Configuration(connector, target, addTo);
    }
    public static ItemConnection AddRotationConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return RotationConnection.Configuration(connector, target, addTo);
    }
    public static ChildConnection AddChildConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return ChildConnection.Configuration(connector, target, addTo);
    }
    public static LuerlockItemConnection AddLuerlockItemConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return LuerlockItemConnection.Configuration(connector, target, addTo);
    }
    public static LuerlockLooseItemConnection AddLuerlockLooseItemConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return LuerlockLooseItemConnection.Configuration(connector, target, addTo);
    }
    public static LuerlockLooseTwoWayItemConnection AddLuerlockLooseTwoWayItemConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return LuerlockLooseTwoWayItemConnection.Configuration(connector, target, addTo);
    }
    public static JointConnection AddJointConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return JointConnection.Configuration(connector, target, addTo);
    }
    #endregion
}
