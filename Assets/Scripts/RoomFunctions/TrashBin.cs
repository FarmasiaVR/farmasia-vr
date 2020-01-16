using UnityEngine;

public class TrashBin : MonoBehaviour {

    #region fields
    //[SerializeField]
    //protected GameObject childCollider;
    [SerializeField]
    private bool sharpTrash;
    #endregion

    //private void Start() {
        //Setup(false);
    //}
    
    //protected void Setup(bool checkForSharp) {
        //CollisionSubscription.SubscribeToTrigger(childCollider, new TriggerListener().OnEnter(collider => EnterTrashbin(collider, checkForSharp)));
    //}

    //private void EnterTrashbin(Collider other, bool checkForSharp) {
        //GeneralItem item = GeneralItem.Find(other.transform);
        //if (item != null) {
            //if (!checkForSharp || item.ObjectType == ObjectType.Needle) {
                //Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
                //item.DestroyInteractable();
            //} else {
                //Logger.Print("Non sharp item placed in trash");
                //Logger.Print(item.ObjectType);
            //}
        //}
    //}

    public void EnterTrashbin(Collider other) {
        GeneralItem item = GeneralItem.Find(other.transform);
        
        if (item != null) {
            if (sharpTrash == (item.ObjectType == ObjectType.Needle)) {
                Events.FireEvent(EventType.ItemDroppedInTrash, CallbackData.Object(item));
            } else {
                Logger.Warning("Item placed in the wrong trash");
                Logger.Print(item.ObjectType);
            }

            Logger.Print("Destroying: " + item);
            item.DestroyInteractable();
        }
    }
}