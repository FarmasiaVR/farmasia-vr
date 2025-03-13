using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AgarPlateVent : MonoBehaviour
{
    public bool isVenting {get; private set; } = false;
    // Change later when rebased on top of changes where bottom can be accessed from lid
    public GameObject bottom;
    private int angleUp = 20;
    private XRSocketInteractor socket;
    private Transform rotationPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (bottom == null)
        {
            Debug.LogWarning("Agar plate bottom unassigned!");
        }
        else { socket = bottom.GetComponentInChildren<XRSocketInteractor>(); }

        rotationPoint = transform.Find("LidRotate");
        if (rotationPoint == null)
        {
            Debug.LogWarning("Couldn't find LidRotate object in children");
        }
    }

    private void StartVenting()
    {
        isVenting = true;
        socket.transform.RotateAround(rotationPoint.position, Vector3.up, angleUp);
    }

    private void StopVenting()
    {
        isVenting = false;
        socket.transform.RotateAround(rotationPoint.position, Vector3.up, -angleUp);
    }

    public void Vent()
    {
        if (rotationPoint == null || socket == null) { return; }
        if (isVenting) { StopVenting(); }
        else { StartVenting(); }
    }

}
