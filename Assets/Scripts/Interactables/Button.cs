public class Button : Interactable {

    #region fields
    private Hand hand;
    private LaminarCabinetButton button;

    #endregion

    protected override void Start() {
        base.Start();
        button = transform.parent.GetComponent<LaminarCabinetButton>();
        Type.Set(InteractableType.Interactable);
    }

    private void Update() {
        if (State == InteractState.Grabbed) {
            button.CheckButtonState();
        }
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Button interact");
        this.hand = hand;
        State.On(InteractState.Grabbed);
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
        this.hand = null;
        State.Off(InteractState.Grabbed);
    }
}