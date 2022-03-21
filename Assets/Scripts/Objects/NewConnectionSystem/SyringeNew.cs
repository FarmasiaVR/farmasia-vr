using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeNew : ReceiverItem {
    public LiquidContainer Container { get; private set; }

    public int LiquidTransferStep = 50;

    public float defaultPosition, maxPosition;

    public Transform handle;

    public LiquidContainer BottleContainer { get; set; }

    public bool hasBeenInBottle;

    [SerializeField]
    private ItemDisplay display;

    protected override void Start() {
        objectType = ObjectType.Syringe;
        base.Start();
        Container = LiquidContainer.FindLiquidContainer(transform);

        Type.On(InteractableType.Interactable);

        //Container.OnAmountChange += SetSyringeHandlePosition;
        //SetSyringeHandlePosition();

        AfterRelease = (interactable) => {
            Logger.Print("Syringe disassembled!");
            Events.FireEvent(EventType.SyringeDisassembled, CallbackData.Object((this, interactable)));
        };

        Logger.Print("GAME OBJECT: " + gameObject);
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);

        display.EnableDisplay();
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabEnd(hand);

        if (State != InteractState.LuerlockAttached && State != InteractState.Grabbed) {
            display.DisableDisplay();
        }
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);

        bool takeMedicine = VRInput.GetControlDown(hand.HandType, Controls.TakeMedicine);
        bool sendMedicine = VRInput.GetControlDown(hand.HandType, Controls.EjectMedicine);

        if (takeMedicine) {
            TakeMedicine();
        } else if (sendMedicine) {
            SendMedicine();
        }

    }

    public void TakeMedicine() {
        if (State == InteractState.InBottle) {
            TransferToBottle(false);
            Events.FireEvent(EventType.TakingMedicineFromBottle, CallbackData.Object(this));
        } else {
            Logger.Print("Pipette not in bottle");
        }
    }

    public void SendMedicine() {
        if (State == InteractState.InBottle) {
            TransferToBottle(true);
            Events.FireEvent(EventType.TakingMedicineFromBottle, CallbackData.Object(this));
        }
    }

    private void TransferToBottle(bool into) {
        if (BottleContainer == null) return;
        if (Vector3.Angle(-BottleContainer.transform.up, transform.up) > 25) return;

        Container.TransferTo(BottleContainer, into ? Container.Capacity : -Container.Capacity);
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