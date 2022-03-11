using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterPart : ReceiverItem
{
    private Dictionary<ObjectType, bool> partsAttached;
    public Dictionary<ObjectType, bool> PartsAttached { get { return partsAttached; } }

    public bool IsAssembled => new List<bool>(partsAttached.Values).TrueForAll(x => x == true);

    protected override void Awake() {
        base.Awake();

        partsAttached = InitParts();
    }

    protected override void ConnectAttachment() {
        base.ConnectAttachment();
        UpdateParts();
    }

    private Dictionary<ObjectType, bool> InitParts() {
        return new Dictionary<ObjectType, bool>() {
        { ObjectType.PumpFilterBase, false },
        { ObjectType.PumpFilterFilter, false },
        { ObjectType.PumpFilterTank, false },
        { ObjectType.PumpFilterLid, false },
    };
    }

    public void UpdateParts() {
        if (SlotOccupied) {
            partsAttached = NearestItem.GetComponent<FilterPart>().PartsAttached;
            partsAttached[ReceivedObjectType] = true;
        } else {
            InitParts();
        }
        if (Attached) (ParentReceiver as FilterPart).UpdateParts();
    }
}
