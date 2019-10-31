using System.Collections.Generic;
using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    public const int RIGHT = 0;
    public const int LEFT = 1;
    private const string luerlockTag = "Luerlock Position";

    #region Fields
    public LuerlockConnector LeftConnector { get => connectors[LEFT]; }
    public LuerlockConnector RightConnector { get => connectors[RIGHT]; }
    private LuerlockConnector[] connectors;

    public List<Rigidbody> AttachedRigidbodies {
        get {
            List<Rigidbody> bodies = new List<Rigidbody>();
            foreach (var connector in connectors) {
                if (connector.AttachedRigidbody != null) {
                    bodies.Add(connector.AttachedRigidbody);
                }
            }
            return bodies;
        }
    }

    private int ObjectCount {
        get {
            int count = 0;
            foreach (var connector in connectors) {
                if (connector.HasAttachedObject) {
                    count++;
                }
            }

            return count;
        }
    }

    public bool HasAttachedObjects { get => ObjectCount > 0; }
    #endregion

    protected override void Start() {
        base.Start();

        ObjectType = ObjectType.Luerlock;
        Type.On(InteractableType.SmallObject);

        connectors = new LuerlockConnector[] {
            new LuerlockConnector(RIGHT, this, transform.Find("Right collider").gameObject),
            new LuerlockConnector(LEFT, this, transform.Find("Left collider").gameObject)
        };

        SubscribeCollisions();
    }

    private void SubscribeCollisions() {
        connectors[RIGHT].Subscribe();
        connectors[LEFT].Subscribe();
    }

    private void OnJointBreak(float breakForce) {
        // Search for the joint that broke
        int index = -1;
        for (int i = 0; i < connectors.Length; i++) {
            Joint joint = connectors[i].Joint;
            if (joint?.currentForce.magnitude == breakForce) {
                index = i;
                break;
            }
        }

        if (index > -1) {
            connectors[index].ReleaseItem();
        }
    }

    private void Update() {
        CheckBreakDistance();
    }

    private void CheckBreakDistance() {
        connectors[RIGHT].CheckObjectDistance();
        connectors[LEFT].CheckObjectDistance();
    }

    public static Transform LuerlockPosition(Transform t) {
        if (t.tag == luerlockTag) {
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
}
