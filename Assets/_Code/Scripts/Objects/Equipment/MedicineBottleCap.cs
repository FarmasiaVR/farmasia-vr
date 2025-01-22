using UnityEngine;

public class MedicineBottleCap : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<DisinfectingCloth>()) {
            gameObject.SetActive(false);
            Bottle bottle = gameObject.GetComponentInParent<Bottle>();
            Events.FireEvent(EventType.BottleDisinfect, CallbackData.Object(bottle));
            
        }
    }
}
