using UnityEngine;
using UnityEngine.Assertions;

public class Pump : GeneralItem
{

    #region fields
    #endregion

    protected override void Start()
    {
        base.Start();
        ObjectType = ObjectType.Pump;
        Type.On(InteractableType.Attachable, InteractableType.Interactable);

    }
}
