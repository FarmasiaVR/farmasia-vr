using UnityEngine;

public class GeneralItem : Grabbable {

    private int flags;

    [SerializeField]
    protected ObjectType objectType = ObjectType.None;
    public virtual ObjectType ObjectType { get => objectType; }

    [SerializeField]
    private bool clean;

    protected override void Start() {
        base.Start();
        SetFlags(clean, ItemState.Status.Clean);
    }

    public void SetFlags(bool value, params ItemState.Status[] statuses) {
        ItemState.SetFlags(ref flags, value, statuses);
    }

    public bool GetFlag(ItemState.Status status) {
        return ItemState.GetFlag(flags, status);
    }
}
