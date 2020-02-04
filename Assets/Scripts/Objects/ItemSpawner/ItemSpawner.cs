using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    [SerializeField]
    GameObject copy;
    GameObject currentObject;

    private void Start() {
        Events.SubscribeToEvent(Copy, EventType.ItemDroppedOnFloor);
        Events.SubscribeToEvent(Copy, EventType.ItemDroppedInTrash);
        if (copy != null) {
            currentObject = copy;
        }
    }

    public void SetCopyObject(GameObject gob) {
        copy = gob;
        currentObject = gob;
    }

    public void Copy(CallbackData data) {
        if (copy == null) {
            return;
        }
        GeneralItem item = (GeneralItem)data.DataObject;
        BreakConnection(item);
        if (item.gameObject == currentObject || currentObject == null) {

            currentObject = Instantiate(copy, transform.position, transform.rotation);
            GeneralItem g = Interactable.GetInteractable(currentObject.transform) as GeneralItem;
            Logger.Warning("ITEM SPAWNER COPIES ITEM CONNECTION EVEN THOUGH IT ABSOLUTELY SHOULD NOT");
            // Below is a budget fix, it works now but might cause problems if the system is changed
            Destroy(g.GetComponent<ItemConnection>());
            g.Contamination = GeneralItem.ContaminateState.Clean;

            //CopyChildStates(Interactable.GetInteractable(copy.transform) as GeneralItem, g);
        }
    }

    private void CopyChildStates(GeneralItem original, GeneralItem copy) {
        if (original.ObjectType == ObjectType.Needle 
            && original as Needle is var needleOriginal 
            && copy as Needle is var needleCopy) {
            if (needleOriginal.Connector.HasAttachedObject) {
                needleCopy.Connector.AttachedInteractable.State = needleOriginal.Connector.AttachedInteractable.State.Copy();
            }
        } else if (original.ObjectType == ObjectType.Luerlock 
            && original as LuerlockAdapter is var luerlockOriginal 
            && copy as LuerlockAdapter is var luerlockCopy) {
            for (int i = 0; i < luerlockOriginal.AttachedInteractables.Count; i++) {
                luerlockCopy.AttachedInteractables[i].State = luerlockOriginal.AttachedInteractables[i].State.Copy();
            }
        }
    }

    private void BreakConnection(GeneralItem item) {

        float amount = 0.4f;

        if (item.ObjectType == ObjectType.Needle && (Needle)item is var needle) {
            if (needle.Connector.HasAttachedObject) {
                Transform connected = needle.Connector.AttachedInteractable.transform;
                Vector3 awayFromNeedle = item.transform.position - connected.transform.position;
                needle.Connector.Connection.Remove();
                connected.transform.position += awayFromNeedle.normalized * amount;
            }
        } else if (item.ObjectType == ObjectType.Luerlock && (LuerlockAdapter)item is var luerlock) {
            if (luerlock.LeftConnector.HasAttachedObject) {
                Transform connected = luerlock.LeftConnector.AttachedInteractable.transform;
                Vector3 awayFromNeedle = item.transform.position - connected.transform.position;
                luerlock.LeftConnector.Connection.Remove();
                connected.transform.position += awayFromNeedle.normalized * amount;
            }
            if (luerlock.RightConnector.HasAttachedObject) {
                Transform connected = luerlock.RightConnector.AttachedInteractable.transform;
                Vector3 awayFromNeedle = item.transform.position - connected.transform.position;
                luerlock.RightConnector.Connection.Remove();
                connected.transform.position += awayFromNeedle.normalized * amount;
            }
        }
    }
}
