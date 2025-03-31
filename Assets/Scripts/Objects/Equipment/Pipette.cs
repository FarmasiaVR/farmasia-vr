using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public class Pipette : GeneralItem {

    public LiquidContainer Container;
    
    // How much liquid is moved per click
    public int LiquidTransferStep = 50;
    
    public float defaultPosition, maxPosition;
    
    public Transform handle;
    [Tooltip("This is called when pipette is contaminated")]
    public UnityEvent<string, int> contaminatedPipeteUsed;
    // The LiquidContainer this Pipette is interacting with
    public LiquidContainer BottleContainer { get; set; }

    public bool hasBeenInBottle;

    [SerializeField]
    private ItemDisplay display;

    protected override void Start() {
        objectType = ObjectType.Pipette;
        base.Start();

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
        bool grabInteract = VRInput.GetControlDown(hand.HandType, Controls.GrabInteract);

        if (takeMedicine) {
            TakeMedicine();
        } else if (sendMedicine) {
            Logger.Print("Hep Hep Hep");
            SendMedicine();
        }

    }

    public void TakeMedicine() {
        if (State == InteractState.InBottle) {
            
            TransferToBottle(false);
        } else {
            Logger.Print("Pipette not in bottle");
        }
    }

    public void SendMedicine() {
        if (State == InteractState.InBottle) {
            Logger.Print("Some other probelem");
            TransferToBottle(true);
        } else {
            Logger.Print("Bottle not found");
        }

    }

    private void TransferToBottle(bool into) {
        
        Logger.Print(into);
        
        if (BottleContainer == null) {
            Logger.Print("Didnt find bottle");    
            return;}
        if (Vector3.Angle(-BottleContainer.transform.up, transform.up) > 25) return;
        Logger.Print("Pippete is trying to get liquid");
        if(Container.contaminationLiquidType != BottleContainer.LiquidType){
            Logger.Print("Doro pippete is contaminated with different liquid type");
            return;
        }
        Container.TransferTo(BottleContainer, into ? Container.Capacity : -Container.Capacity);
    }
    
    public void TakeMedicineXR()
    {
        if (State == InteractState.InBottle)
        {
            TransferToBottleXR(false);
        }
        else
        {
            Logger.Print("Pipette not in bottle");
        }
    }

    public void SendMedicineXR()
    {
        if (State == InteractState.InBottle)
        {
            TransferToBottleXR(true);
        }
    }

    private void TransferToBottleXR(bool into)
    {
        //Logger.Print("This code is runing Doro v2");
        if (BottleContainer == null) return;
        if (Vector3.Angle(-BottleContainer.transform.up, transform.up) > 25) return;
        if (!into && Vector3.Distance(BottleContainer.transform.position, transform.position) > 0.3f) return;
        
        if(Container.contaminationLiquidType != BottleContainer.LiquidType && Container.contaminationLiquidType != LiquidType.None && BottleContainer.LiquidType != LiquidType.None){
            Logger.Print("Pippete is contaminated with different liquid type");
            contaminatedPipeteUsed.Invoke("ContaminatedPipette", 1);
            return;
        }
        Container.TransferTo(BottleContainer, into ? LiquidTransferStep : -LiquidTransferStep);
    }
}
