using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    #region fields
    [SerializeField]
    private GameObject attachedObject1;
    public GameObject AttachedObject1 {
        get => attachedObject1;
        set {
            attachedObject1 = value;
        }
    }

    [SerializeField]
    private GameObject attachedObject2;
    public GameObject AttachedObject2 {
        get => attachedObject2;
        set {
            attachedObject2 = value;
        }
    }
    #endregion

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.Luerlock;
    }
}
