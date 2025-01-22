public class Grabbable : Interactable {

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Grabbable);
    }
}
