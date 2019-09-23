using UnityEngine;

public class MedicineBottle : GeneralItem {

    #region fields
    [SerializeField]
    private int size = 100;
    public int Size {
        get => size;
        set {
            if (value >= 0) {
                size = value;
            }
        } 
    }

    [SerializeField]
    private string content = "medicine";
    public string Content { get => content; }

    [SerializeField]
    private int contentLeft = 100;
    public int ContentLeft {
        get => contentLeft;
        set {
            if (value <= size && value >= 0) {
                contentLeft = value;
            }
        }
    }
    #endregion

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.Bottle;
    }
}
