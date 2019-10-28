using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuerlockConnector : ItemConnector {

    #region fields
    public LuerlockAdapter Luerlock { get; private set; }

    public Joint[] Joints { get; private set; }

    private ItemConnection[] connections;
    #endregion

    public LuerlockConnector(Transform obj) : base(obj) {
        Luerlock = obj.GetComponent<LuerlockAdapter>();
        Joints = new Joint[2];
        connections = new ItemConnection[2];
    }

    private Joint Joint(int side) {

        if (Joints[side] == null) {
            Joints[side] = JointConfiguration.AddJoint(Luerlock.gameObject);
        }

        return Joints[side];
    }

    

    #region Attaching
    public override void ConnectItem(Interactable interactable, int side) {

        Logger.Print("Connect item: " + interactable.name);

        if (Luerlock.State == InteractState.Grabbed) {
            Hand.GrabbingHand(Luerlock.Rigidbody).Connector.ReleaseItem(0);
        }

        ReplaceObject(side, interactable?.gameObject);
    }

    private void ReplaceObject(int side, GameObject newObject) {

        GameObject colliderT = Luerlock.Colliders[side];

        LuerlockAdapter.AttachedObject obj = Luerlock.Objects[side];

        if (obj.GameObject != null) {

            if (obj.GameObject == newObject) {
                Logger.Print("Attaching same object!");
                return;
            }

            CollisionIgnore.IgnoreCollisions(Luerlock.transform, obj.GameObject.transform, false);

            Joint(side).connectedBody = null;
        }

        if (newObject == null) {
            obj.Interactable = null;
            Luerlock.Objects[side] = obj;

            Logger.Print("New object null!");
            return;
        }

        obj.Interactable = newObject.GetComponent<Interactable>();
        obj.Scale = newObject.transform.localScale;

        obj.Interactable.Interactors.SetLuerlockPair(new KeyValuePair<int, LuerlockAdapter>(side, Luerlock));

        Logger.PrintVariables("luerlock", Luerlock.name);
        Logger.PrintVariables("obj luer: ", obj.Interactable.Interactors.LuerlockPair.Value.gameObject.name);

        obj.Interactable.State.On(InteractState.LuerlockAttached);

        CollisionIgnore.IgnoreCollisions(Luerlock.transform, obj.GameObject.transform, true);


        // Attaching

        Luerlock.Objects[side] = obj;

        SetLuerlockPosition(colliderT, obj.GameObject.transform);

        Joint(side).connectedBody = obj.Rigidbody;
        connections[side] = ItemConnection.AddRotationConnection(this, Luerlock.Colliders[side].transform, obj.GameObject);
        //connections[side] = ItemConnection.AddRigidConnection(this, Luerlock.Colliders[side].transform, obj.GameObject);
    }

    private void SetLuerlockPosition(GameObject collObject, Transform t) {

        Transform target = LuerlockAdapter.LuerlockPosition(t);

        if (target == null) {
            throw new System.Exception("Luerlock position not found");
        }

        Vector3 offset = collObject.transform.position - target.position;
        t.position += offset;
    }
    #endregion

    #region Releasing
    public override void ReleaseItem(int side) {

        //if (luerlock.Interactable.State != InteractState.Grabbed) {
        //    throw new System.Exception("Trying to release ungrabbed item");
        //}
        Events.FireEvent(EventType.SyringeFromLuerlock, CallbackData.Object(Luerlock.Objects[side]));

        Joint(side).connectedBody = null;
        MonoBehaviour.Destroy(Joint(side));
        MonoBehaviour.Destroy(connections[side]);
        Luerlock.Objects[side].Interactable.Interactors.SetLuerlockPair(new KeyValuePair<int, LuerlockAdapter>(-1, null));
        Luerlock.Objects[side].Interactable.State.Off(InteractState.LuerlockAttached);
        ReplaceObject(side, null);
    }
    #endregion
}