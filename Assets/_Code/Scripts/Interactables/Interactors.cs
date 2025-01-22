using System.Collections.Generic;
using UnityEngine;

public struct Interactors {

    public Hand Hand;
    public KeyValuePair<LuerlockAdapter.Side, LuerlockAdapter> LuerlockPair { get; private set; }
    public ConnectableItem ConnectableItem;

    public void SetHand(Hand hand) {
        Hand = hand;
    }
    
    public void SetLuerlockPair(LuerlockAdapter.Side side, LuerlockAdapter luerlock) {
        if (luerlock == null) {
            // Luerlock pair.Value can only be null when removing the syringe from luerlock, therefore Warning instead of Error
            Logger.Error("Luerlock pair value was null. Problem with SetInteractors cast?");
        }
        LuerlockPair = new KeyValuePair<LuerlockAdapter.Side, LuerlockAdapter>(side, luerlock);
    }
    public void ResetLuerlockPair() {
        LuerlockPair = new KeyValuePair<LuerlockAdapter.Side, LuerlockAdapter>(LuerlockAdapter.Side.Left, null);
    }
    
    public void SetConnectableItem(ConnectableItem item) {
        ConnectableItem = item;
    }

    public void ResetConnectableItem() {
        ConnectableItem = null;
    }


}