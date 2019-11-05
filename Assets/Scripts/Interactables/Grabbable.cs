public class Grabbable : Interactable {

    public override void Interact(Hand hand) {
        throw new System.NotImplementedException();
    }

    protected override void Awake_Interactable() {
        Awake_Grabbable();
    }

    protected override void Start_Interactable() {
        Type.On(InteractableType.Grabbable);
        Start_Grabbable();
    }

    protected virtual void Awake_Grabbable() {}
    protected virtual void Start_Grabbable() {}
}
