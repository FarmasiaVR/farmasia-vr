using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class AgarPlateVent : MonoBehaviour
{
    public bool isVenting {get; private set; } = false;
    private AgarPlateLid lid;
    private GameObject bottomObject;
    private AgarPlateBottom bottom;
    private int angleUp = 20;
    private XRSocketInteractor socket;
    private Transform rotationPoint;

    public UnityEvent<bool> onVentingChanged;

    // Start is called before the first frame update
    void Start()
    {
        lid = GetComponent<AgarPlateLid>();
        bottomObject = lid.PlateBottom;
        if (bottomObject == null)
        {
            Debug.LogWarning("Agar plate bottom unassigned!");
        }
        else
        {
            socket = bottomObject.GetComponentInChildren<XRSocketInteractor>();
            bottom = bottomObject.GetComponent<AgarPlateBottom>();
            rotationPoint = bottomObject.transform.Find("LidRotate");
            if (rotationPoint == null)
            {
                Debug.LogWarning("Couldn't find LidRotate object in children");
            }
        }
    }

    private void StartVenting()
    {
        isVenting = true;
        bottom.isVenting = true;
        socket.transform.RotateAround(rotationPoint.position, Vector3.up, angleUp);
    }

    private void StopVenting()
    {
        isVenting = false;
        bottom.isVenting = false;
        socket.transform.RotateAround(rotationPoint.position, Vector3.up, -angleUp);
    }

    private void VentingChanged()
    {
        onVentingChanged.Invoke(isVenting);
    }

    public void Vent()
    {
        if (rotationPoint == null || bottomObject == null || bottom.isOpen) { return; }
        if (isVenting) { StopVenting(); }
        else { StartVenting(); }
        VentingChanged();
    }

    public void StopVentingAfterOpening()
    {
        if (!isVenting) { return; }
        StopVenting();
        VentingChanged();
    }

}
