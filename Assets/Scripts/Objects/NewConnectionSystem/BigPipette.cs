using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is <c>ReceiverItem</c> for PipetteContainer and transfers controller press events to it.
/// </summary>
public class BigPipette : ReceiverItem
{
    public PipetteContainer pipetteContainerXR;


    public override void OnGrabStart(Hand hand) {
        base.OnGrabStart(hand);

        if (SlotOccupied) {
            (ConnectedItem as PipetteContainer).Display.EnableDisplay();
        }
    }

    public override void OnGrabEnd(Hand hand) {
        base.OnGrabEnd(hand);

        if (SlotOccupied) {
            (ConnectedItem as PipetteContainer).Display.DisableDisplay();
        }
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);

        if (SlotOccupied) {
            bool takeMedicine = VRInput.GetControlDown(hand.HandType, Controls.TakeMedicine);
            bool sendMedicine = VRInput.GetControlDown(hand.HandType, Controls.EjectMedicine);
            bool grabInteract = VRInput.GetControlDown(hand.HandType, Controls.GrabInteract);
            if (takeMedicine) {
                TakeMedicine();
            } else if (sendMedicine) {
                SendMedicine();
            }
        }
    }
   
    public void TakeMedicine() {
        // Debug.Log("Big pipette starts taking medicine");
        if (ConnectedItem)
        {
            (ConnectedItem as PipetteContainer).TakeMedicine();
        }
        
    }

      public void SendMedicine() {
        if (ConnectedItem) (ConnectedItem as PipetteContainer).SendMedicine();
    }

    public void setConnectedItem(PipetteContainer NewConnectedItem)
    {
        ConnectedItem = NewConnectedItem;
    }

}
