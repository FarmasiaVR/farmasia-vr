using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemConnection : MonoBehaviour {

    protected abstract ItemConnector Connector { get; set; }

    protected abstract void FixedUpdate();

    #region Static methods
    public static ItemConnection AddSmoothConnection(ItemConnector connector, Transform target, GameObject addTo) {
        return SmoothConnection.Configuration(connector, target, addTo);
    }
    public static ItemConnection AddRigidConnection(ItemConnector connector, Transform target, GameObject addTo) {
        return null;
    }
    #endregion
}
