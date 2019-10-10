using UnityEngine;
using UnityEngine.Assertions;

public class Syringe : GeneralItem {

    #region fields
    public LiquidContainer Container { get; private set; }

    [SerializeField]
    private float defaultPosition, maxPosition;

    [SerializeField]
    private Transform handle;

    private float liquidEjectSpeed = 5000;

    private const float SWIPE_DEFAULT_TIME = 0.75f;
    private float swipeTime;

    public LiquidContainer BottleContainer { get; set; }

    // private Pipeline pipeline = new Pipeline();

    #endregion

    protected override void Start() {
        base.Start();
        Container = LiquidContainer.FindLiquidContainer(transform);
        Assert.IsNotNull(Container);
        ObjectType = ObjectType.Syringe;

        Type.On(InteractableType.LuerlockAttachable, InteractableType.HasLiquid, InteractableType.Interactable);

        Container.OnAmountChange += SetSyringeHandlePosition;

        SetSyringeHandlePosition();
    }

    public override void Interact(Hand hand) {
        swipeTime = SWIPE_DEFAULT_TIME;
    }

    public override void UpdateInteract(Hand hand) {

        bool padTouchUp = VRInput.GetControlUp(hand.HandType, ControlType.PadTouch);
        bool touch = VRInput.GetControl(hand.HandType, ControlType.PadTouch);
        bool padTouchDown = VRInput.GetControlDown(hand.HandType, ControlType.PadTouch);

        if (touch && swipeTime > 0) {
            swipeTime = SWIPE_DEFAULT_TIME;
        } else {
            swipeTime -= Time.deltaTime;
        }

        if (padTouchDown || padTouchUp || !touch || swipeTime <= 0) {
            return;
        }

        float changeFactor = -VRInput.PadTouchDelta(hand.HandType).y;
        int amount = (int)(changeFactor * liquidEjectSpeed * Time.deltaTime);

        if (amount == 0) {
            return;
        }

        if (State == InteractState.LuerlockAttatch) {
            LuerlockEject(amount);
        } else if (State == InteractState.InBottle) {
            BottleEject(amount);
        } else {
            Eject(amount);
        }
    }

    private void Eject(int amount) {
        Container.SetAmount(amount + Container.Amount);
    }
    private void LuerlockEject(int amount) {

        var pair = Interactors.LuerlockPair;

        if (pair.Key < 0) {
            return;
        }

        int other = pair.Key == 0 ? 1 : 0;

        if (pair.Value == null) {
            return;
        }

        LiquidContainer from = ((Syringe)(pair.Value.Objects[other].Interactable)).Container;
        LiquidContainer to = ((Syringe)(pair.Value.Objects[pair.Key].Interactable)).Container;

        if (amount > 0) {
            from.TransferTo(to, amount);
        } else {
            to.TransferTo(from, -amount);
        }
    }
    private void BottleEject(int amount) {

        if (Vector3.Angle(-BottleContainer.transform.up, transform.up) > 25) {
            return;
        }

        if (BottleContainer == null) {
            return;
        }

        if (amount > 0) {
            BottleContainer.TransferTo(Container, amount);
        } else {
            Container.TransferTo(BottleContainer, -amount);
        }
    }

    public void SetSyringeHandlePosition() {
        Vector3 pos = handle.localPosition;
        pos.y = SyringePos();
        handle.localPosition = pos;
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
