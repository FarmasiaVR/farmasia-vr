using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreakSubscription : MonoBehaviour {

    public delegate void JointBreakCallback(float force);
    private JointBreakCallback callback;

    private void OnJointBreak(float breakForce) {
        callback?.Invoke(breakForce);
        Destroy(this);
    }

    public static void Subscribe(GameObject g, JointBreakCallback callback) {
        JointBreakSubscription s = g.AddComponent<JointBreakSubscription>();
        s.callback = callback;
    }
}
