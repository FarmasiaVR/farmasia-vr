using UnityEngine;

public class GeneralItem : Grabbable {

    #region fields
    [SerializeField]
    protected ObjectType objectType = ObjectType.None;
    public ObjectType ObjectType { get => objectType; set { objectType = value; } }
    public bool isTriggered;
    public enum ContaminateState {
        Clean,
        Contaminated,
        FloorContaminated
    }

    [SerializeField]
    public bool isClean = true;
    private ContaminateState contamination;
    public ContaminateState Contamination {
        get => contamination; 
        set {
            if (value == ContaminateState.Clean) {
                contamination = ContaminateState.Clean;
            } else if (value == ContaminateState.Contaminated) {
                if (contamination != ContaminateState.FloorContaminated) {
                    contamination = ContaminateState.Contaminated;
                }
            } else {
                contamination = ContaminateState.FloorContaminated;
            }
        }
    }
    public bool IsClean { get => Contamination == ContaminateState.Clean; }
    #endregion

    protected override void Start() {
        base.Start();
        Contamination = isClean ? ContaminateState.Clean : ContaminateState.Contaminated;
    }

    public static GeneralItem Find(Transform t) {
        return Interactable.GetInteractableObject(t)?.GetComponent<GeneralItem>();
    }


    /// <summary>
    /// Is called when this item collides with another
    /// </summary>
    /// <param name="coll"></param>
    protected virtual void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Floor") {
            Contamination = ContaminateState.FloorContaminated;
            State.On(InteractState.OnFloor);
            Events.FireEvent(EventType.ItemDroppedOnFloor, CallbackData.Object(this));
        }
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);

        if (IsOnFloor) {
            State.Off(InteractState.OnFloor);
        }
    }
}
