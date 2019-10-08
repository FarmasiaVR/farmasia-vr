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

    private float liquidEjectSpeed = 250;

    private bool swipeEnable;
    #endregion

    protected override void Start() {
        base.Start();
        Container = LiquidContainer.FindLiquidContainer(transform);
        Assert.IsNotNull(Container);
        ObjectType = ObjectType.Syringe;

        Type.On(InteractableType.LuerlockAttachable, InteractableType.HasLiquid);

        Container.OnAmountChange += SetSyringeHandlePosition;
    }

    public override void Interact(Hand hand) {
        swipeEnable = true;
    }

    public override void UpdateInteract(Hand hand) {

        if (VRInput.GetControlUp(hand.HandType, ControlType.PadTouch)) {
            swipeEnable = false;
        }
        if (VRInput.GetControlDown(hand.HandType, ControlType.PadTouch) || VRInput.GetControlUp(hand.HandType, ControlType.PadTouch) || swipeEnable) {
            return;
        }

        float changeFactor = VRInput.PadTouchDelta(hand.HandType).y / 2;

        int amount = (int)(changeFactor * liquidEjectSpeed * Time.deltaTime) + Container.Amount;

        Container.SetAmount(amount);
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
