using UnityEngine;

public class GeneralItem : Grabbable {

    #region fields
    protected ObjectType objectType = ObjectType.None;
    public ObjectType ObjectType { get => objectType; set { objectType = value; } }

    [SerializeField]
    private bool isClean = true;
    public bool IsClean { get => isClean; set => isClean = value; }
    #endregion

    public static GeneralItem Find(Transform t) {
        return Interactable.GetInteractableObject(t)?.GetComponent<GeneralItem>();
    }

    protected virtual void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Floor") IsClean = false;
    }
}
