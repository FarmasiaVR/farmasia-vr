using UnityEngine;

public class TrashCanCollider : MonoBehaviour {

    private TrashCan trashCan;

    private void Awake() {
        trashCan = transform.GetComponentInParent<TrashCan>();
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("item entered trashCanCollider");
        if (other.isTrigger) return;
        trashCan.OnTrashEnter(other);
    }
}
