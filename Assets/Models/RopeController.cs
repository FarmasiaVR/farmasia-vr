using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    [SerializeField]
    private Transform ControlPoint;

    private GeneralItem GenItem;

    private void Awake() {
        GenItem = GetComponent<GeneralItem>();
    }

    private void LateUpdate() {
        if (GenItem.IsGrabbed) {
            ControlPoint.position = transform.position;
            ControlPoint.rotation = transform.rotation;
        }
    }
}
