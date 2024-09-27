﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;


public class LaminarCabinetTable : MonoBehaviour {

    #region fields
    private static float ContaminateTime = 0.75f;
    private HashSet<int> contaminatedItems;

    private TriggerInteractableContainer safeZone;
    private TriggerInteractableContainer contaminateZone;
    #endregion

    private void Start() {
        safeZone = transform.GetChild(0).gameObject.AddComponent<TriggerInteractableContainer>();
        contaminateZone = transform.GetChild(1).gameObject.AddComponent<TriggerInteractableContainer>();
        contaminatedItems = new HashSet<int>();
    }

    #region Collision events
    private void OnCollisionEnter(Collision coll) {
        if (Interactable.GetInteractable(coll.gameObject.transform) as GeneralItem is var i && i != null) {
            ContaminateItem(i);
        }
    }
    #endregion

    private void ContaminateItem(GeneralItem item) {
        if (!safeZone.Contains(item) && contaminateZone.Contains(item)) {
            StartCoroutine(Wait());
        }

        IEnumerator Wait() {
            yield return new WaitForSeconds(ContaminateTime);

            if (safeZone.Contains(item) || !contaminateZone.Contains(item)) {
                yield break;
            }

            if (item.IsClean || !contaminatedItems.Contains(item.GetInstanceID())) {
                contaminatedItems.Add(item.GetInstanceID());
                Task.CreateGeneralMistake(Translator.Translate("XR MembraneFilteration 2.0", "ItemTouchedCabinet"));
                item.Contamination = GeneralItem.ContaminateState.Contaminated;
            }
        }
    }
}
