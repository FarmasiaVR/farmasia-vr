using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PipetteContainer : AttachmentItem
{
    public LiquidContainer Container;

    // How much liquid is moved per click
    public int LiquidTransferStep = 10000;

    public float defaultPosition, maxPosition;

    public Transform handle;

    // The LiquidContainer this Pipette is interacting with
    public LiquidContainer BottleContainer { get; set; }

    public bool hasBeenInBottle;

    [SerializeField]
    private ItemDisplay display;
    public ItemDisplay Display { get { return display; } set { display = value; } }

    protected override void Start() {
        base.Start();

        Type.On(InteractableType.Interactable);
    }

    public void TakeMedicine() {
        if (State == InteractState.InBottle) {
            TransferToBottle(false);
        } else {
            Logger.Print("PipetteContainer not in bottle");
        }
    }

    public void SendMedicine() {
        if (State == InteractState.InBottle) {
            TransferToBottle(true);
        } else {
            Eject();
        }
    }

    private void Eject() {
        Container.SetAmount(0);
    }

    private void TransferToBottle(bool into) {
        if (BottleContainer == null) return;
        //if (Vector3.Angle(-BottleContainer.transform.up, transform.up) > 25) return;

        Container.TransferTo(BottleContainer, into ? LiquidTransferStep : -LiquidTransferStep);
    }
}
