using UnityEngine;
using UnityEngine.Assertions;

public class Syringe : GeneralItem {

    #region Constants
    private const float SWIPE_DEFAULT_TIME = 0.75f;
    private const float LIQUID_TRANSFER_SPEED = 15;
    #endregion

    #region fields
    public LiquidContainer Container { get; private set; }

    [SerializeField]
    private float defaultPosition, maxPosition;

    [SerializeField]
    private Transform handle;

    

    private float swipeTime;

    public LiquidContainer BottleContainer { get; set; }

    public bool hasBeenInBottle;

    // private Pipeline pipeline = new Pipeline();

    #endregion
    protected override void Start() {
        base.Start();

        Container = LiquidContainer.FindLiquidContainer(transform);
        Assert.IsNotNull(Container);
        ObjectType = ObjectType.Syringe;
        IsClean = true;

        //Type.On(InteractableType.LuerlockAttachable, InteractableType.HasLiquid, InteractableType.Interactable);
         Type.On(InteractableType.LuerlockAttachable, InteractableType.HasLiquid, InteractableType.Interactable, InteractableType.SmallObject);
        

        Container.OnAmountChange += SetSyringeHandlePosition;
        SetSyringeHandlePosition();

        hasBeenInBottle = false;
    }

    public override void Interact(Hand hand) {
        swipeTime = SWIPE_DEFAULT_TIME;
    }

    public override void Interacting(Hand hand) {

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
        int amount = (int)(changeFactor * LIQUID_TRANSFER_SPEED * Container.Capacity * Time.deltaTime);

        if (amount == 0) {
            return;
        }

        if (State == InteractState.LuerlockAttached) {
            LuerlockEject(amount);
        } else if (State == InteractState.InBottle) {
            BottleEject(amount);
        } else {
            Eject(amount);
        }
    }

    private void Eject(int amount) {
        if (amount < 0) {
            Container.SetAmount(amount + Container.Amount);
        }
    }
    private void LuerlockEject(int amount) {

        var pair = Interactors.LuerlockPair;

        if (pair.Key < 0 || pair.Value == null) {
            return;
        }

        Syringe leftSyringe = (Syringe)pair.Value.LeftConnector.AttachedInteractable;
        Syringe rightSyringe = (Syringe)pair.Value.RightConnector.AttachedInteractable;
        bool invert = (pair.Key == 0) == (amount < 0);

        Syringe srcSyringe = invert ? rightSyringe : leftSyringe;
        Syringe dstSyringe = invert ? leftSyringe : rightSyringe;
        srcSyringe.Container.TransferTo(dstSyringe.Container, Mathf.Abs(amount));
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
