using UnityEngine;

public class CleanUpObject : MonoBehaviour {

    public CabinetBase laminarCabinet;

    public void EnableCleanUp() {
        // Find and destroy all item factories
        ObjectFactory.DestroyAllFactories(true);
        foreach (ItemSpawner interactable in FindObjectsOfType<ItemSpawner>()) {
            Destroy(interactable.gameObject);
        }
        // Make syringe cap bag interactable
        foreach (Interactable interactable in laminarCabinet.GetContainedItems()) {
            if (interactable as GeneralItem is var item && item != null) {
                if (item is SyringeCapBag) {
                    item.GetComponent<SyringeCapBag>().EnableSyringeCapBag();
                    continue;
                }
            }
        }
    }

    public static CleanUpObject GetCleanUpObject() {
        return FindObjectOfType<CleanUpObject>();
    }
}
