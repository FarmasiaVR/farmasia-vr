using UnityEngine;

public class GeneralItem : Grabbable {

    #region fields
    private int flags;

    [SerializeField]
    protected ObjectType objectType = ObjectType.None;
    public ObjectType ObjectType { get => objectType; set { objectType = value; } }

    [SerializeField]
    private bool isClean;
    #endregion

    protected override void Start() {
        base.Start();
        SetFlags(isClean, ItemState.Status.Clean);
    }

    public void SetFlags(bool value, params ItemState.Status[] statuses) {
        ItemState.SetFlags(ref flags, value, statuses);
    }

    public bool GetFlag(ItemState.Status status) {
        return ItemState.GetFlag(flags, status);
    }
}
