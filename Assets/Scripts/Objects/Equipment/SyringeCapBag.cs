public class SyringeCapBag : GeneralItem {
    
    protected override void Start() {
        base.Start();

        ObjectType = ObjectType.SyringeCapBag;

        Type.On(InteractableType.Interactable);
    }
}
