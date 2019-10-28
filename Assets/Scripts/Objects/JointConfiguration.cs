using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JointConfiguration {

    private static float breakForce = 20000;
    private static float breakTorque = 20000;

    public static Joint AddFixedJoint(GameObject obj) {
        FixedJoint joint = obj.AddComponent<FixedJoint>();

        joint.breakForce = breakForce;
        joint.breakTorque = breakTorque;

        return joint;
    }

    public static Joint AddConfigurableJoint(GameObject obj) {
        return obj.AddComponent<ConfigurableJoint>();
    }

    public static Joint AddJoint(GameObject obj) {
         return AddFixedJoint(obj);
    }
}
