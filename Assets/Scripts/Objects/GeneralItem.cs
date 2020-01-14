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
        if (coll.gameObject.tag == "Floor") {
            IsClean = false;
            State.On(InteractState.OnFloor);
            Events.FireEvent(EventType.ItemDroppedOnFloor, CallbackData.Object(this));
        }
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);

        if (IsOnFloor) {
            Events.FireEvent(EventType.ItemLiftedOffFloor, CallbackData.Object(this));
            State.Off(InteractState.OnFloor);
        }
    }
}
