using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeNew : ReceiverItem {

    public LiquidContainer Container { get; private set; }

    // How much liquid is moved per click
    public int LiquidTransferStep = 50;

    public float defaultPosition, maxPosition;

    public Transform handle;

    // The LiquidContainer this Pipette is interacting with
    public LiquidContainer BottleContainer { get; set; }

    public bool hasBeenInBottle;

    /*[SerializeField]
    private ItemDisplay display;*/

    private GameObject liquidDisplay;
    private GameObject currentDisplay;
    private bool displayState;
    protected override void Start() {
        base.Start();
        GameObject cap = gameObject.transform.parent.GetChild(1).gameObject;
        Container = LiquidContainer.FindLiquidContainer(transform);

        Type.On(InteractableType.Interactable);

        Container.OnAmountChange += SetSyringeHandlePosition;
        SetSyringeHandlePosition();

        AfterRelease = (interactable) => {
            Logger.Print("Syringe disassembled!");
            Events.FireEvent(EventType.SyringeDisassembled, CallbackData.Object((this, interactable)));
        };

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
    }

    public void DisableDisplay() {
        if (currentDisplay != null) {
            Destroy(currentDisplay);
        }

        displayState = false;
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        EnableDisplay();
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabEnd(hand);

        if (State != InteractState.Grabbed) {
            DisableDisplay();
        }
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);

        bool takeMedicine = VRInput.GetControlDown(hand.HandType, Controls.TakeMedicine);
        bool sendMedicine = VRInput.GetControlDown(hand.HandType, Controls.EjectMedicine);

        int liquidAmount = 0;

        if (takeMedicine) liquidAmount -= LiquidTransferStep;
        if (sendMedicine) liquidAmount += LiquidTransferStep;
        if (liquidAmount == 0) return;

        if (SlotOccupied) {
            return;
        }

        if (takeMedicine) {
            TakeMedicine(liquidAmount);
        } else if (sendMedicine) {
            SendMedicine(liquidAmount);
        }

    }

    public void TakeMedicine(int amount) {
        if (State == InteractState.InBottle) {
            Logger.Print("Take medicine: " + amount);
            TransferToBottle(amount);
            SetSyringeHandlePosition();
            Logger.Print("Nestettä ruiskussa: " + Container.Amount);
        } else {
            Logger.Print("Syringe not in bottle");
        }
    }

    public void SendMedicine(int amount) {
        if (State == InteractState.InBottle) {
            Logger.Print("Sending medicine: " + amount);
            TransferToBottle(amount);
            Logger.Print("Nestettä ruiskussa: " + Container.Amount);
        } else {
            //Eject();
        }
        SetSyringeHandlePosition();
    }

    private void Eject() {
        Container.SetAmount(0);
    }

    private void TransferToBottle(int amount) {
        if (BottleContainer == null) return;
        //if (Vector3.Angle(-BottleContainer.transform.up, transform.up) > 25) return;
        Logger.Print("Transfer to: " + BottleContainer.transform.parent);
        Container.TransferTo(BottleContainer, amount);
        Logger.Print("Amount in bottle container: " + BottleContainer.Amount);
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