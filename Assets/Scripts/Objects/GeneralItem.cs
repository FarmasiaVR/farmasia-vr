using UnityEngine;

public class GeneralItem : Grabbable {

    #region fields
    [SerializeField]
    protected ObjectType objectType = ObjectType.None;
    public ObjectType ObjectType { get => objectType; set { objectType = value; } }

    [SerializeField]
    private bool isClean;
    public bool IsClean { get => isClean; set => isClean = value; }
    #endregion

    public static GeneralItem Find(Transform t) {
        return Interactable.GetInteractableObject(t)?.GetComponent<GeneralItem>();
    }

    protected override void Awake_Grabbable() {
        Awake_GeneralItem();
    }

    protected override void Start_Grabbable() {
        Start_GeneralItem();
    }

    protected virtual void Awake_GeneralItem() {}
    protected virtual void Start_GeneralItem() {}

    protected virtual void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Floor") IsClean = false;
    }
}
