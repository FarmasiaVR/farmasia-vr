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
    [SerializeField] private GameObject pipette;
    private Collider triggerCollider;

    void Awake()
    {        
        audioSource = GetComponent<AudioSource>();        
        triggerCollider = GetComponentInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {      
        
        if (other.gameObject == pipette)
        {
            audioSource.Play();
            Pipette pippetescript = other.GetComponent<Pipette>();
            pippetescript.Container.contaminationLiquidType = LiquidType.None;
            pippetescript.Container.LiquidType = LiquidType.None;
            pippetescript.Container.amount = 0;             
            
        }
    }
}
