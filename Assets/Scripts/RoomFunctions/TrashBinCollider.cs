using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBinCollider : MonoBehaviour {

    #region fields
    TrashBin mainScript;
    #endregion

    private void Awake() {
        mainScript = transform.GetComponentInParent<TrashBin>();
    }

    private void OnTriggerEnter(Collider other) {
        mainScript.EnterTrashbin(other);
    }
}
