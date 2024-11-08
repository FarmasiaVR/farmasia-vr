using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CleaningBottleColliderPCM : MonoBehaviour
{
    public PlateCountMethodSceneManager sceneManager;
    public GameObject Effect;
    private List<GeneralItem> Items = new List<GeneralItem>();
    private new ParticleSystem particleSystem;
    private bool canCleanCabinet;

    //this is a quick prototype version for cleaning hands, it will be improved TM =)
    bool handInCollider = false;

    private void Awake() {
        particleSystem = Effect.GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other) {
        GeneralItem item = other.GetComponentInParent<GeneralItem>();
        if (!(item == null)) {
            Items.Add(item);
        }
        if (other.CompareTag("LaminarCabinet")) canCleanCabinet = true;

        //this is a quick prototype version for cleaning hands, it will be improved TM =)
        if (other.GetComponent<XRBaseController>() != null) handInCollider = true;
    }

    private void OnTriggerExit(Collider other) {
        Items.Remove(other.GetComponentInParent<GeneralItem>());
        if (other.CompareTag("LaminarCabinet")) canCleanCabinet = false;

        //this is a quick prototype version for cleaning hands, it will be improved TM =)
        if (other.GetComponent<XRBaseController>() != null) handInCollider = false;
    }

    public void Clean() {
        if (canCleanCabinet) Events.FireEvent(EventType.CleaningBottleSprayed, CallbackData.Object(this));
        particleSystem.Play();
        bool cleaned = false;
        foreach (GeneralItem item in Items) {
            cleaned |= item.Contamination == GeneralItem.ContaminateState.Contaminated || item.Contamination == GeneralItem.ContaminateState.FloorContaminated;
            item.Contamination = GeneralItem.ContaminateState.Clean;
        }
        if (cleaned) {
            transform.parent.GetComponentInChildren<AudioSource>().Play();
        }

        //this is a quick prototype version for cleaning hands, it will be improved TM =)
        if (handInCollider)
        {
            HandStateManager handStateManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HandStateManager>();
            if (handStateManager)
            {
                handStateManager.cleanHands();
                sceneManager.CleanHands();
            }
        }
    }
}
