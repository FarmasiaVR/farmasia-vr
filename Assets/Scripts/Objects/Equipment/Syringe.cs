using UnityEngine;
using UnityEngine.Assertions;

public class Syringe : GeneralItem {

    #region fields
    public LiquidContainer Container { get; private set; }

    [SerializeField]
    private float defaultPosition, maxPosition;

    [SerializeField]
    private Transform handle;

    private Vector3 lastPos;
    #endregion

    protected override void Start() {
        base.Start();
        Container = LiquidContainer.FindLiquidContainer(transform);
        Assert.IsNotNull(Container);
        ObjectType = ObjectType.Syringe;

        Types.On(InteractableType.LuerlockAttachable, InteractableType.HasLiquid);

        Container.OnAmountChange += SetSyringeHandlePosition;
    }

    public void SetSyringeHandlePosition() {
        Vector3 pos = handle.localPosition;
        pos.y = SyringePos();
        handle.localPosition = pos;
        lastPos = pos;
    }

    private float SyringePos() {
        return Factor * (maxPosition - defaultPosition);
    }

    private float Factor {
        get {
            return 1.0f * Container.Amount / Container.Capacity;
        }
    }
}
