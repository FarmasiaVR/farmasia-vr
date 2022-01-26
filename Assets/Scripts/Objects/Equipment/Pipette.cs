using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipette : GeneralItem {
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
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
