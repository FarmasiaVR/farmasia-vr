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

    public static Joint AddJoint(GameObject obj) {

         return AddFixedJoint(obj);

        ConfigurableJoint joint = obj.AddComponent<ConfigurableJoint>();

        joint.breakForce = breakForce;
        joint.breakTorque = breakTorque;

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.projectionMode = JointProjectionMode.PositionAndRotation;

        Logger.Print("Returning joint");
        return joint;
    }
}
