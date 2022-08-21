using UnityEngine;

public class MedicineBottleCap : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.name.Equals("SterilizationCloth(Clone)")) {
            gameObject.SetActive(false);
        }
    }
}
