using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemConnection : MonoBehaviour {

    protected abstract ItemConnector Connector { get; set; }

    protected abstract void OnDestroy();

    protected virtual void Update() {

    }

    #region Static methods
    public static ItemConnection AddSmoothConnection(ItemConnector connector, Transform target, GameObject addTo) {
        return SmoothConnection.Configuration(connector, target, addTo);
    }
    public static ItemConnection AddRigidConnection(ItemConnector connector, Transform target, GameObject addTo) {
        return RigidConnection.Configuration(connector, target, addTo);
    }
    public static ItemConnection AddRotationConnection(ItemConnector connector, Transform target, GameObject addTo) {
        return RotationConnection.Configuration(connector, target, addTo);
    }
    public static ChildConnection AddChildConnection(ItemConnector connector, Transform target, GameObject addTo) {
        return ChildConnection.Configuration(connector, target, addTo);
    }
    public static LuerlockItemConnection AddLuerlockItemConnection(HandConnector connector, Transform target, GameObject addTo) {
        return LuerlockItemConnection.Configuration(connector, target, addTo);
    }
    public static JointConnection AddJointConnection(HandConnector connector, Transform target, GameObject addTo) {
        return JointConnection.Configuration(connector, target, addTo);
    }

    /// <summary>
    /// Removes the connection from the attached object g.
    /// </summary>
    /// <param name="g"></param>
    public static void RemoveConnection(GameObject g) {

        ItemConnection conn = g.GetComponent<ItemConnection>();

        if (conn != null) {
            MonoBehaviour.Destroy(conn);
        }
    }
    public static void RemoveConnection<T>(GameObject g) where T : ItemConnection {

        ItemConnection conn = g.GetComponent<T>();

        if (conn != null) {
            MonoBehaviour.Destroy(conn);
        }
    }

    
    #endregion
}
