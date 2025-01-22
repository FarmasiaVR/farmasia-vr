using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PumpFilterFilter : FilterPart {
    public bool CanBeCut { 
        get {
            // Logger.Print(Attached + " " + ConnectedItem);
            return !isCut; 
        } 
    }
    private bool isCut = false;

    public GameObject wholeFilterToDisable;
    public GameObject leftHalfToEnable;
    public GameObject leftHalfSpawnPoint;
    public GameObject rightHalfToEnable;
    public GameObject rightHalfSpawnPoint;

    [Header("Events")]
    public UnityEvent onFilterCut;


    public void Cut(Transform bladeTransform) {

        RotateToBlade(bladeTransform);

        transform.GetChild(0).gameObject.SetActive(false); // Uncut filter
        var leftHalf = transform.GetChild(2).gameObject;
        var rightHalf = transform.GetChild(3).gameObject;
        leftHalf.SetActive(true);
        rightHalf.SetActive(true);
        transform.DetachChildren();

        isCut = false;
        Events.FireEvent(EventType.FilterCutted, CallbackData.Object(this));
    }

    public void cutXR(Transform bladeTransform)
    {
        RotateToBlade(bladeTransform);
        wholeFilterToDisable.SetActive(false);
        leftHalfToEnable.SetActive(true);
        leftHalfToEnable.transform.position = leftHalfSpawnPoint.transform.position;
        leftHalfToEnable.transform.rotation = leftHalfSpawnPoint.transform.rotation;
        rightHalfToEnable.SetActive(true);
        rightHalfToEnable.transform.position = rightHalfSpawnPoint.transform.position;
        rightHalfToEnable.transform.rotation = rightHalfSpawnPoint.transform.rotation;
        Events.FireEvent(EventType.FilterCutted, CallbackData.Object(this));
        onFilterCut.Invoke();
    }



    // This does not really work, yet!
    private void RotateToBlade(Transform bladeTransform) {
        var bladeDown = bladeTransform.forward;
        var relativeBladeDirection = Vector3.ProjectOnPlane(bladeDown, transform.up);
        var angleDifference = Vector3.Angle(relativeBladeDirection, transform.right);
        transform.Rotate(0f, angleDifference, 0f, Space.Self);
    }
}
