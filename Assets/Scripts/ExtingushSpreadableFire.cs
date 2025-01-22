using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtingushSpreadableFire : MonoBehaviour
{
    public float streamLenght = 15f, streamWidth = 0.5f,extinguishRadius=1f;
    public LayerMask mask;
    public Transform streamOrigin;

    bool extinguish = false;
    public float tickrate = 0.1f;
    float tick = 0;
    public void Update()
    {
        if (!extinguish)
            return;
        if (tick < tickrate)
        {
            tick += Time.deltaTime;
            return;
        }
        tick = 0;
        if (!streamOrigin)
            return;
        if (Physics.SphereCast(streamOrigin.position, streamWidth, streamOrigin.forward, out RaycastHit hit, streamLenght, mask))
        {
            SpreadableFire spreadableFire = hit.collider.GetComponent<SpreadableFire>();
            if (spreadableFire)
                spreadableFire.ExtinguishVerticesInRaidus(hit.point, extinguishRadius);
        }
    }
    public void Extinguish()
    {
        extinguish = true;
    }
    public void StopExtinguishing()
    {
        extinguish = false;
    }
}
