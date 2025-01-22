using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSimpleInteractable))]
public class DragAcceptableToXRConverter : MonoBehaviour
{
    /// <summary>
    /// A simple script that converts a DragAcceptable object to work with the XR library.
    /// </summary>
    /// 

    private XRBaseInteractable XRinteractable;
    private DragAcceptable legacyInteractable;
    private void Start()
    {
        XRinteractable = GetComponent<XRBaseInteractable>();

        XRinteractable.selectEntered.AddListener(OnAccept);
    }

    private void OnAccept(SelectEnterEventArgs eventArgs)
    {
        legacyInteractable = GetComponent<DragAcceptable>();
        legacyInteractable.OnAccept();
    }
}
