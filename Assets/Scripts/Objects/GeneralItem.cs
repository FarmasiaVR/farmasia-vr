using UnityEngine;

public class GeneralItem : Grabbable {

    #region fields
    [SerializeField]
    protected ObjectType objectType = ObjectType.None;
    public ObjectType ObjectType { get => objectType; set { objectType = value; } }


    [SerializeField]
    private bool isClean;
    public bool IsClean { get => isClean; private set => isClean = value; }
    #endregion
}
