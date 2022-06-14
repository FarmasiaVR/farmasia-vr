using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// no rigidbody, Object must not be grabbable. Only collision with hand

public class SoapDispencer : GeneralItem {
    public GameObject Effect;
    private new ParticleSystem particleSystem;

    protected override void Awake() {
        base.Awake();
        particleSystem = Effect.GetComponent<ParticleSystem>();
    }
    
    public void OnCollisionEnter(Collider other) {
        // on collision with hands, apply soap.
        ApplySoap();
        Debug.Log("Testing Collision with hand");

    }

    public void ApplySoap() {
        particleSystem.Play();

        bool soaped = false;
        /*
        foreach (GeneralItem item in Items) {
            cleaned |= item.Contamination == GeneralItem.ContaminateState.Contaminated || item.Contamination == GeneralItem.ContaminateState.FloorContaminated;
            item.Contamination = GeneralItem.ContaminateState.Clean;
        }

        */
        if (soaped) {
            // Play audio
            // transform.parent.GetComponentInChildren<AudioSource>().Play();
        }
    }
}
