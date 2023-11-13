using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BunsenburnerFire : MonoBehaviour
{
    public float flameLength = 0.1f;
    public LayerMask mask;
    public Transform flameOrigin;
    public VisualEffect flameEffect;

    bool burning = false;
    private void Awake()
    {
        TurnOff();
    }
    public void Update()
    {
        if (!burning)
            return;
        if (!flameOrigin)
            return;
        if (Physics.Raycast(flameOrigin.position, flameOrigin.forward, out RaycastHit hit, flameLength, mask))
        {
            SpreadableFire spreadableFire = hit.collider.GetComponent<SpreadableFire>();
            if (spreadableFire)
                spreadableFire.IgniteClosestVertexNonCollision(hit.point);
        }
    }
    public void TurnOn()
    {
        if (flameEffect)
            flameEffect.Play();
        burning = true;
    }
    public void TurnOff()
    {
        if (flameEffect)
            flameEffect.Stop();
        burning = false;
    }
    public void ToggleFlame()
    {
        if (burning)
            TurnOff();
        else
            TurnOn();
    }
}
