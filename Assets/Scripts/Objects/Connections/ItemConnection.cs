﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemConnection : MonoBehaviour {

    protected bool removed;

    protected abstract ItemConnector Connector { get; set; }

    public void Remove() {
        if (removed) {
            Logger.Print("Re-Removing item conn");
            return;
        }
        removed = true;
        OnRemoveConnection();
        Destroy(this);
        Connector.OnReleaseItem();
    }

    protected virtual void OnRemoveConnection() { }

    #region Static methods
    public static ChildConnection AddChildConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return ChildConnection.Configuration(connector, target, addTo);
    }
    public static LuerlockItemConnection AddLuerlockItemConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return LuerlockItemConnection.Configuration(connector, target, addTo);
    }
    public static LuerlockLooseItemConnection AddLuerlockLooseItemConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return LuerlockLooseItemConnection.Configuration(connector, target, addTo);
    }
    public static JointConnection AddJointConnection(ItemConnector connector, Transform target, Interactable addTo) {
        return JointConnection.Configuration(connector, target, addTo);
    }
    #endregion
}
