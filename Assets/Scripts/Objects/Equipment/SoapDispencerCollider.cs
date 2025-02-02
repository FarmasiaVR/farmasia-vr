using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoapDispencerCollider : MonoBehaviour {
    public GameObject Effect;
    private List<GeneralItem> Items = new List<GeneralItem>();
    private new ParticleSystem particleSystem;

    private void Awake() {
        particleSystem = Effect.GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other) {
        // If collision with hand
        GeneralItem item = other.GetComponentInParent<GeneralItem>();
        if (!(item == null)) {
            Items.Add(item);
        }
    }

    private void OnTriggerExit(Collider other) {
        // If exit collision with hand
        Items.Remove(other.GetComponentInParent<GeneralItem>());
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
