using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

public class Syringe : GeneralItem {

    public LiquidContainer Container { get; private set; }

    [SerializeField]
    private int LiquidTransferStep = 50;

    [SerializeField]
    private float defaultPosition, maxPosition;

    [SerializeField]
    private Transform handle;

    private GameObject syringeCap;
    public bool capVisible;
    public bool HasSyringeCap { get { return syringeCap.activeInHierarchy; } }

    public LiquidContainer BottleContainer { get; set; }

    public bool hasBeenInBottle;


    private GameObject liquidDisplay;
    private GameObject currentDisplay;
    private bool displayState;

    protected override void Start() {
        base.Start();

        Container = LiquidContainer.FindLiquidContainer(transform);
        Assert.IsNotNull(Container);
        ObjectType = ObjectType.Syringe;

        Type.On(InteractableType.Attachable, InteractableType.Interactable);

        Container.OnAmountChange += SetSyringeHandlePosition;
        SetSyringeHandlePosition();

        hasBeenInBottle = false;

        // Only check for the cap if using the legacy system.
        if (!GetComponent<XRBaseInteractable>())
        {
            syringeCap = transform.Find("syringe_cap").gameObject;
            NullCheck.Check(syringeCap);

            syringeCap.SetActive(capVisible);
        }

        liquidDisplay = Resources.Load<GameObject>("Prefabs/LiquidDisplay");
        displayState = false;
    }

    public void EnableDisplay() {
        if (displayState) {
            return;
        }

        displayState = true;
        currentDisplay = Instantiate(liquidDisplay);
        LiquidDisplay display = currentDisplay.GetComponent<LiquidDisplay>();
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
        bool sendMedicine = VRInput.GetControlDown(hand.HandType, Controls.EjectMedicine);

        int liquidAmount = 0;

        if (takeMedicine) liquidAmount -= LiquidTransferStep;
        if (sendMedicine) liquidAmount += LiquidTransferStep;
        if (liquidAmount == 0) return;

        if (this.HasSyringeCap) {
            Logger.Warning("Cannot change liquid amount of syringe with a cap");
            return;
        }
            Logger.Print("Taking medicine");

        if (State == InteractState.LuerlockAttached && Interactors.LuerlockPair.Value.ObjectCount == 2) {
            TransferToLuerlock(liquidAmount);
        } else if (State == InteractState.InBottle) {
            TransferToBottle(liquidAmount);
        } else {
            Eject(liquidAmount);
        }
    }

    private void Eject(int amount) {
        if (amount > 0) Container.SetAmount(Container.Amount - amount);
    }

    private void TransferToLuerlock(int amount) {
        bool pushing = amount > 0;

        var pair = Interactors.LuerlockPair;

        if (pair.Key < 0 || pair.Value == null) {
            return;
        }

        Syringe other = (Syringe)pair.Value.LeftConnector.AttachedInteractable != this ?
            (Syringe)pair.Value.LeftConnector.AttachedInteractable :
            (Syringe)pair.Value.RightConnector.AttachedInteractable;

        if (pushing) {
            if (other.Container.Capacity < Container.Capacity) {
                Events.FireEvent(EventType.PushingToSmallerSyringe);
            }
        }

        Container.TransferTo(other.Container, amount);
    }

    private void TransferToBottle(int amount) {
        if (BottleContainer == null) return;
        Debug.Log("survived bottle container null check");
        if (Vector3.Angle(-BottleContainer.transform.up, transform.up) > 25) return;
        Debug.Log("survived angle check");
        Container.TransferTo(BottleContainer, amount);
    }

    public void takeMedicineFromBottleXR()
    {
        Debug.Log("taking medicine!");
        TransferToBottle(-LiquidTransferStep);
    }

    public void sendMedicineToBottleXR()
    {
        TransferToBottle(LiquidTransferStep);
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
