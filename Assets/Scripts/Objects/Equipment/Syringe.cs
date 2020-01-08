using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class Syringe : GeneralItem {

    #region Constants
    private const float SWIPE_DEFAULT_TIME = 0.75f;
    private const float LIQUID_TRANSFER_SPEED = 15;
    private const int LIQUID_TRANSFER_STEP = 50; // 0.1ml
    #endregion

    #region fields
    public LiquidContainer Container { get; private set; }

    [SerializeField]
    private float defaultPosition, maxPosition;

    [SerializeField]
    private Transform handle;

    [SerializeField]
    private GameObject syringeCap;
    public bool HasSyringeCap { get { return syringeCap.activeInHierarchy; } }

    public LiquidContainer BottleContainer { get; set; }

    public bool hasBeenInBottle;


    private GameObject liquidDisplay;
    private GameObject currentDisplay;
    private bool displayState;

    private int bottleEjectIndex = 0;
    private const float BOTTLE_EJECT_TIMEOUT = 3f;
    #endregion
    protected override void Start() {
        base.Start();

        Container = LiquidContainer.FindLiquidContainer(transform);
        Assert.IsNotNull(Container);
        ObjectType = ObjectType.Syringe;

        Type.On(InteractableType.LuerlockAttachable, InteractableType.HasLiquid, InteractableType.Interactable, InteractableType.SmallObject);

        Container.OnAmountChange += SetSyringeHandlePosition;
        SetSyringeHandlePosition();

        hasBeenInBottle = false;

        syringeCap.SetActive(false);

        liquidDisplay = Resources.Load<GameObject>("Prefabs/LiquidDisplay");
        displayState = false;
    }

    public void EnableDisplay() {
        if (displayState) {
            return;
        }

        displayState = true;
        currentDisplay = Instantiate(liquidDisplay);
        SyringeDisplay display = currentDisplay.GetComponent<SyringeDisplay>();
        display.SetFollowedObject(gameObject);

        EnableForOtherSyringeDisplay();
    }

    public void DisableDisplay() {
        if (State != InteractState.LuerlockAttached && State != InteractState.Grabbed) {
            DestroyDisplay();
        }
    }

    public void DestroyDisplay() {
        if (currentDisplay != null) {
            Destroy(currentDisplay);
        }

        displayState = false;
    }

    private void EnableForOtherSyringeDisplay() {
        if (State == InteractState.LuerlockAttached && (Interactors.LuerlockPair.Value.ObjectCount == 2)) {
            Syringe other = (Syringe)Interactors.LuerlockPair.Value.GetOtherInteractable(this);
            other.EnableDisplay();
        }
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);

        EnableDisplay();
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabEnd(hand);

        DisableDisplay();
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);

        bool takeMedicine = VRInput.GetControlDown(hand.HandType, Controls.TakeMedicine);
        bool ejectMedicine = VRInput.GetControlDown(hand.HandType, Controls.EjectMedicine);

        int ejectAmount = 0;
        if (takeMedicine) ejectAmount = -LIQUID_TRANSFER_STEP;
        if (ejectMedicine) ejectAmount = LIQUID_TRANSFER_STEP;

        // If nothing is being transfered, why waste time every frame? Will this if statement cause problems?
        if (ejectAmount == 0) {
            return;
        }

        if (this.HasSyringeCap) {
            Logger.Print("Cannot change liquid amount of syringe with a cap");
            return;
        }

        if (State == InteractState.LuerlockAttached && Interactors.LuerlockPair.Value.ObjectCount == 2) {
            LuerlockEject(ejectAmount);
        } else if (State == InteractState.InBottle) {
            BottleEject(ejectAmount);
        } else {
            Eject(ejectAmount);
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
        bool invert = (pair.Key == 0) != (amount < 0);

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

        bottleEjectIndex++;
        CheckFinalEject(bottleEjectIndex);
    }

    private async void CheckFinalEject(int index) {

        await Task.Delay(TimeSpan.FromSeconds(BOTTLE_EJECT_TIMEOUT));

        if (bottleEjectIndex == index) {
            Events.FireEvent(EventType.FinishedTakingMedicineToSyringe, CallbackData.Object(this));
        }
    }

    public void SetSyringeHandlePosition() {
        Vector3 pos = handle.localPosition;
        pos.y = SyringePos();
        handle.localPosition = pos;
    }

    public void ShowSyringeCap(bool show) {
        syringeCap.SetActive(show);
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
