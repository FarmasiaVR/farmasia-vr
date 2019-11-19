
public class Needle : GeneralItem {
    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.Needle;
        IsClean = true;
        Type.On(InteractableType.Interactable, InteractableType.SmallObject);
    }
}
