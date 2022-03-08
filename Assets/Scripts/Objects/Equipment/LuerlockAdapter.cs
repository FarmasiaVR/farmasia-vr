using System;
using System.Collections.Generic;
using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    public enum Side {
        Left, Right
    }

    #region Constants
    private const string LUERLOCK_TAG = "Luerlock Position";
    #endregion

    #region Fields
    public LuerlockConnector LeftConnector { get => GetConnector(Side.Left); }
    public LuerlockConnector RightConnector { get => GetConnector(Side.Right); }
    private Dictionary<Side, LuerlockConnector> connectors;

    public List<Rigidbody> AttachedRigidbodies {
        get {
            List<Rigidbody> bodies = new List<Rigidbody>();
            foreach (var pair in connectors) {
                if (pair.Value.AttachedRigidbody != null) {
                    bodies.Add(pair.Value.AttachedRigidbody);
                }
            }
            return bodies;
        }
    }
    public List<Interactable> AttachedInteractables {
        get {
            List<Interactable> ints = new List<Interactable>();
            foreach (var pair in connectors) {
                if (pair.Value.AttachedInteractable != null) {
                    ints.Add(pair.Value.AttachedInteractable);
                }
            }
            return ints;
        }
    }

    public int ObjectCount {
        get {
            int count = 0;
            foreach (var pair in connectors) {
                if (pair.Value.HasAttachedObject) {
                    count++;
                }
            }

            return count;
        }
    }

    public bool HasAttachedObjects { get => ObjectCount > 0; }
    public int GrabbedObjectCount {
        get {
            if (ObjectCount == 0) {
                return 0;
            }

            int count = 0;

            if (LeftConnector.HasAttachedObject && LeftConnector.AttachedInteractable.State == InteractState.Grabbed) {
                count++;
            }
            if (RightConnector.HasAttachedObject && RightConnector.AttachedInteractable.State == InteractState.Grabbed) {
                count++;
            }

            return count;
        }
    }
    #endregion

    protected override void Start() {
        base.Start();

        ObjectType = ObjectType.Luerlock;
        Contamination = ContaminateState.Clean;

        connectors = new Dictionary<Side, LuerlockConnector> {
            { Side.Left, new LuerlockConnector(Side.Left, this, transform.Find("Left collider").gameObject) },
            { Side.Right, new LuerlockConnector(Side.Right, this, transform.Find("Right collider").gameObject) }
        };

        SubscribeCollisions();
    }

    private void SubscribeCollisions() {
        LeftConnector.Subscribe();
        RightConnector.Subscribe();
    }

    public LuerlockConnector GetConnector(Side side) {
        LuerlockConnector value;
        return connectors.TryGetValue(side, out value) ? value : null;
    }

    public static Transform LuerlockPosition(Transform t) {
        if (t.tag == LUERLOCK_TAG) {
            return t;
        }

        foreach (Transform c in t) {
            Transform l = LuerlockPosition(c);

            if (l != null) {
                return l;
            }
        }

        return null;
    }
    public Interactable GetOtherInteractable(Interactable interactable) {

        bool exists = LeftConnector.AttachedInteractable == interactable || RightConnector.AttachedInteractable == interactable;
        if (!exists) {
            throw new Exception("Interactable is not attached to luerlock");
        }

        return LeftConnector.AttachedInteractable == interactable ? RightConnector.AttachedInteractable : LeftConnector.AttachedInteractable;
    }
    public LuerlockConnector GetConnector(Interactable interactable) {
        if (LeftConnector.AttachedInteractable == interactable) {
            return LeftConnector;
        } else if (RightConnector.AttachedInteractable == interactable) {
            return RightConnector;
        }
        throw new Exception("Connector not found");
    }

    public void ReleaseItems() {
        LeftConnector.Connection?.Remove();
        RightConnector.Connection?.Remove();
    }
}
