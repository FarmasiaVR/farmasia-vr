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
        base.Start();

        Container = LiquidContainer.FindLiquidContainer(transform);
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
    
    
}
