using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningBottleCollider : MonoBehaviour
{
    public GameObject Effect;
    private List<GeneralItem> Items = new List<GeneralItem>();
    private new ParticleSystem particleSystem;

    private void Awake() {
        particleSystem = Effect.GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other) {
        GeneralItem item = other.GetComponentInParent<GeneralItem>();
        if (!(item == null)) {
            Items.Add(item);
        }
        if (other.CompareTag("LaminarCabinet")) Events.FireEvent(EventType.CleaningBottleSprayed, CallbackData.Object(this));
    }

    private void OnTriggerExit(Collider other) {
        Items.Remove(other.GetComponentInParent<GeneralItem>());
    }

    public void Clean() {
        particleSystem.Play();
        bool cleaned = false;
        foreach (GeneralItem item in Items) {
            cleaned |= item.Contamination == GeneralItem.ContaminateState.Contaminated || item.Contamination == GeneralItem.ContaminateState.FloorContaminated;
            item.Contamination = GeneralItem.ContaminateState.Clean;
        }
        if (cleaned) {
            transform.parent.GetComponentInChildren<AudioSource>().Play();
        }
    }
}
