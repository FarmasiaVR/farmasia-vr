using UnityEngine;

public class TrashBin : MonoBehaviour {

    #region fields
    [SerializeField]
    private bool sharpTrash;
    #endregion

    public void EnterTrashbin(Collider other) {
        GeneralItem item = GeneralItem.Find(other.transform);
        
        if (item != null) {
            bool isNeedle = item.ObjectType == ObjectType.Needle;
            if (sharpTrash == isNeedle) {
                Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
            } else {
                Logger.Warning("Item placed in the wrong trash: " + item.ObjectType);
            }

            item.DestroyInteractable();
        }
    }
}