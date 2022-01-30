using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipette : GeneralItem {
    
    public LiquidContainer Container { get; private set; }
    
    private int LiquidTransferStep = 50;
    
    private float defaultPosition, maxPosition;
    
    public Transform handle;
    
    private GameObject liquidDisplay;

    private GameObject currentDisplay;
    private bool displayState;
    
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        Container = LiquidContainer.FindLiquidContainer(transform);

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

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);

        EnableDisplay();
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabEnd(hand);

        DisableDisplay();
    }
    
    
}
