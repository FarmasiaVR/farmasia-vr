public class SyringeCapBag : GeneralItem {
    
    protected override void Start() {
        base.Start();

        ObjectType = ObjectType.SyringeCapBag;
        IsClean = true;

        Type.On(InteractableType.Interactable);
    }
}
