using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JointConfiguration {

    private static float breakForce = 2000;
    private static float breakTorque = 2000;
    private static float damper = 100;
    private static float spring = 10000;

    public static Joint AddFixedJoint(GameObject obj) {
        FixedJoint joint = obj.AddComponent<FixedJoint>();

        joint.breakForce = breakForce;
        joint.breakTorque = breakTorque;

        return joint;
    }

    public static Joint AddConfigurableJoint(GameObject obj) {

        ConfigurableJoint joint = obj.AddComponent<ConfigurableJoint>();

        return joint;
    }

    public static Joint AddJoint(GameObject obj) {
         return AddFixedJoint(obj);
    }

    public static Joint AddSpringJoint(GameObject gameObject) {

        SpringJoint joint = gameObject.AddComponent<SpringJoint>();

        joint.breakForce = breakForce;
        joint.breakTorque = breakTorque;

        joint.damper = damper;
        joint.spring = spring;

        return joint;
    }
}
