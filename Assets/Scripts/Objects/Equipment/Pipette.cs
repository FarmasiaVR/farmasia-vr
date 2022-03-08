using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipette : GeneralItem {
    
    public LiquidContainer Container { get; private set; }
    
    // How much liquid is moved per click
    public int LiquidTransferStep = 50;
    
    public float defaultPosition, maxPosition;
    
    public Transform handle;

    // The LiquidContainer this Pipette is interacting with
    public LiquidContainer BottleContainer { get; set; }

    public bool hasBeenInBottle;

    [SerializeField]
    private ItemDisplay display;

    protected override void Start() {
        objectType = ObjectType.Pipette;
        base.Start();

        Container = LiquidContainer.FindLiquidContainer(transform);

        
        Type.On(InteractableType.Interactable);
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
        } else {
            Eject();
        }
    }

    private void Eject() {
        Container.SetAmount(0);
    }

    private void TransferToBottle(bool into) {
        if (BottleContainer == null) return;
        if (Vector3.Angle(-BottleContainer.transform.up, transform.up) > 25) return;

        Container.TransferTo(BottleContainer, into ? Container.Capacity : -Container.Capacity);
    }
    
}
