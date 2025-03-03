using System;
using UnityEngine.Assertions;
using FarmasiaVR.Legacy;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;
using JetBrains.Annotations;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipette_box : MonoBehaviour
{
    public AudioSource audioSource;
    public OpenCloseBox box;
    [SerializeField] private GameObject pipette;
    private Collider triggerCollider;

    void Awake()
    {        
        audioSource = GetComponent<AudioSource>();        
        triggerCollider = GetComponentInChildren<Collider>();
        box = GetComponent<OpenCloseBox>();

    }

    private void OnTriggerEnter(Collider other)
    {      
        
        if (other.gameObject == pipette && box.isOpen)
        {
            audioSource.Play();
            Pipette pippetescript = other.GetComponent<Pipette>();
            pippetescript.Container.contaminationLiquidType = LiquidType.None;
            pippetescript.Container.LiquidType = LiquidType.None;
            pippetescript.Container.amount = 0;             
            
        }
    }
}
