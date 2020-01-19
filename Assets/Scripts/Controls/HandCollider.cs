using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandCollider : MonoBehaviour {

    #region Fields
    private HashSet<Interactable> grabObjects;
    public ObjectHighlight PreviousHighlight { get; private set; }

    private enum ModelType {
        None = -1,
        Vive = 2,
        Index = 4
    }

    private static Vector3 INDEX_POS = new Vector3(0, 0, -0.0863f);
    private static Vector3 VIVE_POS = new Vector3(0, -0.03f, 0.015f);
    #endregion

    private void Start() {
        grabObjects = new HashSet<Interactable>();

        StartCoroutine(SetCollPos());
    }

    private IEnumerator SetCollPos() {

        while (true) {
            var type = GetModelType();

            yield return null;

            if (type == SteamVR_TrackedObject.EIndex.None) {
                continue;
            }

            if (BIsHTCViveController(type)) {
                Logger.Print("Using VIVE controller");
                transform.localPosition = VIVE_POS;
            } else {
                Logger.Print("Using INDEX controller");
                transform.localPosition = INDEX_POS;
            }

            break;
        }
    }

    bool BIsHTCViveController(SteamVR_TrackedObject.EIndex type) {
        System.Text.StringBuilder sbType = new System.Text.StringBuilder(1000);
        Valve.VR.ETrackedPropertyError err = Valve.VR.ETrackedPropertyError.TrackedProp_Success;
        SteamVR.instance.hmd.GetStringTrackedDeviceProperty((uint)type, Valve.VR.ETrackedDeviceProperty.Prop_ManufacturerName_String, sbType, 1000, ref err);
        return (err == Valve.VR.ETrackedPropertyError.TrackedProp_Success && sbType.ToString().StartsWith("HTC"));
    }

    private SteamVR_TrackedObject.EIndex GetModelType() {
        Transform model = transform.parent.Find("Model");
        SteamVR_RenderModel m = model.GetComponent<SteamVR_RenderModel>();
        return m.index;
    }

    private void OnTriggerEnter(Collider coll) {
        Interactable interactable = Interactable.GetInteractable(coll.transform);
        if (interactable != null) {
            grabObjects.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider coll) {
        Interactable interactable = Interactable.GetInteractable(coll.transform);
        if (interactable == null) {
            return;
        }

        ObjectHighlight highlight = ObjectHighlight.GetHighlightFromTransform(coll.transform);
        if (highlight == PreviousHighlight) UnhighlightPrevious();
        grabObjects.Remove(interactable);
    }

    public void RemoveInteractable(Interactable interactable) {
        if (grabObjects.Contains(interactable)) {
            grabObjects.Remove(interactable);
        }
    }

    #region Highlight
    public void HighlightClosestObject() {
        HighlightObject(GetClosestObject());
    }

    public void HighlightPointedObject(float maxAngle) {
        HighlightObject(GetPointedObject(maxAngle));
    }

    public void UnhighlightAll() {
        foreach (Interactable child in grabObjects) {
            ObjectHighlight.GetHighlightFromTransform(child.transform)?.Unhighlight();
        }
    }

    private void HighlightObject(Interactable obj) {
        UnhighlightPrevious();

        if (grabObjects.Contains(obj)) {
            PreviousHighlight = ObjectHighlight.GetHighlightFromTransform(obj.transform);
            PreviousHighlight?.Highlight();
        }
    }
    #endregion

    public void UnhighlightPrevious() {
        PreviousHighlight?.Unhighlight();
        PreviousHighlight = null;
    }

    public Interactable GetClosestInteractable() {
        return GetClosestObject()?.GetComponent<Interactable>() ?? null;
    }

    public Interactable GetClosestObject() {
        float closestDistance = float.MaxValue;
        Interactable closest = null;

        foreach (Interactable rb in grabObjects) {
            float distance = Vector3.Distance(transform.position, rb.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = rb;
            }
        }

        return closest;
    }

    public Interactable GetPointedObject(float maxAngle) {
        float smallestAngle = float.MaxValue;
        Interactable closest = null;

        foreach (Interactable g in grabObjects) {
        grabObjects.Remove(null);

            if (g == null) {
                Logger.Print("G was null");
                continue;
            } else if (g.transform == null) {
                Logger.Print("G.Transform was null");
            }
            float angle = Vector3.Angle(transform.forward, g.transform.position - transform.position);

            if (angle < smallestAngle && angle < maxAngle) {
                smallestAngle = angle;
                closest = g;
            }
        }

        return closest;
    }
}
