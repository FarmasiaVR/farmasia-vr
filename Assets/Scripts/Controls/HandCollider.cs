using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandCollider : MonoBehaviour {

    #region Fields
    public ObjectHighlight PreviousHighlight { get; private set; }

    private TriggerInteractableContainer container;
    private Collider collider;

    private enum ModelType {
        None = -1,
        Vive = 2,
        Index = 4
    }

    private static Vector3 INDEX_POS = new Vector3(0, 0, -0.0863f);
    private static Vector3 VIVE_POS = new Vector3(0, -0.03f, 0.015f);
    #endregion

    private void Start() {
        container = gameObject.AddComponent<TriggerInteractableContainer>();
        container.OnExit = OnInteractableExit;

        collider = GetComponent<Collider>();

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


    private void OnInteractableExit(Interactable interactable) {
        if (interactable.Highlight == PreviousHighlight) UnhighlightPrevious();
    }

    public void Enable(bool enable) {
        collider.enabled = enable;
        if (!enable) {
            container.ResetContainer();
        }
    }

    public void RemoveInteractable(Interactable interactable) {
        container.EnteredObjects.Remove(interactable);
    }

    #region Highlight
    public void HighlightClosestObject() {
        HighlightObject(GetClosestObject());
    }

    public void HighlightPointedObject(float maxAngle) {
        HighlightObject(GetPointedObject(maxAngle));
    }

    public void UnhighlightAll() {
        foreach (Interactable child in container.Objects) {
            child.Highlight.Unhighlight();
        }
    }

    private void HighlightObject(Interactable obj) {
        UnhighlightPrevious();

        if (container.Contains(obj)) {
            PreviousHighlight = obj.Highlight;
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

        foreach (Interactable rb in container.Objects) {
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

        foreach (Interactable interactable in container.Objects) {

            if (interactable == null) {
                Logger.Print("interactable was null");
                continue;
            }

            float angle = Vector3.Angle(transform.forward, interactable.transform.position - transform.position);

            if (angle < smallestAngle && angle < maxAngle) {
                smallestAngle = angle;
                closest = interactable;
            }
        }

        return closest;
    }
}
