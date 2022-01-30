using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipette : GeneralItem {
    
    public LiquidContainer Container { get; private set; }
    
    private int LiquidTransferStep = 50;
    
    private float defaultPosition, maxPosition;
    
    public Transform handle;
    
    private GameObject liquidDisplay;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        liquidDisplay = Resources.Load<GameObject>("Prefabs/LiquidDisplay");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);
        if (!IsClean) {
            Logger.Print("Pipette grabbed and it was dirty!");
        }
        
    }
    
    
}
