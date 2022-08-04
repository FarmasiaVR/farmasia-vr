using UnityEngine;

public class TrashCanCollider : MonoBehaviour {

    private TrashCan trashCan;

    private void Awake() {
        trashCan = transform.GetComponentInParent<TrashCan>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger) return;
        trashCan.OnTrashEnter(other);
    }
}
