﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JointConfiguration {

    private static float breakForce = 1000;
    private static float breakTorque = 1000;
    private static float damper = 100;
    private static float spring = 10000;

    private static Joint AddFixedJoint(GameObject obj) {
        FixedJoint joint = obj.AddComponent<FixedJoint>();

        float mass = obj.GetComponent<Rigidbody>()?.mass ?? 1;

        joint.breakForce = breakForce * mass;
        joint.breakTorque = breakTorque * mass;

        return joint;
    }

    private static Joint AddConfigurableJoint(GameObject obj) {

        ConfigurableJoint joint = obj.AddComponent<ConfigurableJoint>();

        return joint;
    }

    public static Joint AddJoint(GameObject obj, float mass) {
         return AddFixedJoint(obj);
    }

    private static Joint AddSpringJoints(GameObject gameObject, float mass) {

        SpringJoint joint = gameObject.AddComponent<SpringJoint>();

        joint.breakForce = breakForce * mass;
        joint.breakTorque = breakTorque * mass;

        joint.damper = damper;
        joint.spring = spring;

        return joint;
    }
}
