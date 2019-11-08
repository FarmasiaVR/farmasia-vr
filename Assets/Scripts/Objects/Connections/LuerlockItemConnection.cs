using System;
using UnityEngine;

public class LuerlockItemConnection : ItemConnection {

    #region Fields
    protected override ItemConnector Connector { get; set; }
    private Transform target;
    #endregion

    private void Start() {

    }

    protected override void RemoveConnection() {
        Connector.OnReleaseItem();
    }

    public static LuerlockItemConnection Configuration(HandConnector connector, Transform target, GameObject addTo) {

        LuerlockItemConnection conn = addTo.AddComponent<LuerlockItemConnection>();

        conn.Connector = connector;
        conn.target = target;



        return conn;
    }
}
