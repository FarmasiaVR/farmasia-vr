using System;
using UnityEngine.Assertions;
using FarmasiaVR.Legacy;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;
using JetBrains.Annotations;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipette_box : GeneralItem
{
    public AudioSource audioSource;
    public OpenCloseBox box;
    private int pipetteLayer;
    private Collider triggerCollider;
    public UnityEvent onPipetteCollided;

    protected override void Awake()
    {        
        audioSource = GetComponent<AudioSource>();        
        triggerCollider = GetComponentInChildren<Collider>();
        box = GetComponent<OpenCloseBox>();
        pipetteLayer = LayerMask.NameToLayer("Pipette");
    }

    private void OnTriggerEnter(Collider other)
    {      
        
        if (other.gameObject.layer == pipetteLayer && box.isOpen)
        {
            audioSource.Play();
            Pipette pippetescript = other.GetComponent<Pipette>();
            pippetescript.Container.contaminationLiquidType = LiquidType.None;
            pippetescript.Container.LiquidType = LiquidType.None;            
            pippetescript.Container.SetAmount(0);       
            onPipetteCollided?.Invoke();      
            
        }
    }
}
