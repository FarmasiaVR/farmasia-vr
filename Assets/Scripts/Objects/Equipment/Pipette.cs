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

    private GameObject liquidDisplay;
    private GameObject currentDisplay;

    private bool isDisplayOn;

    protected override void Start() {
        base.Start();

        Container = LiquidContainer.FindLiquidContainer(transform);

        liquidDisplay = Resources.Load<GameObject>("Prefabs/LiquidDisplay");
        isDisplayOn = false;
    }

    public void EnableDisplay() {
        if (isDisplayOn) {
            return;
        }
        isDisplayOn = true;
        currentDisplay = Instantiate(liquidDisplay);
        SyringeDisplay display = currentDisplay.GetComponent<SyringeDisplay>();
        display.SetFollowedObject(gameObject);

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

        isDisplayOn = false;
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);

        EnableDisplay();
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabEnd(hand);

        DisableDisplay();
    }
    
    
}
