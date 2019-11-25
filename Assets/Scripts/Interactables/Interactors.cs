using System.Collections.Generic;
using UnityEngine;

public struct Interactors {

    public Hand Hand;
    public KeyValuePair<LuerlockAdapter.Side, LuerlockAdapter> LuerlockPair;
    public GameObject Bottle;
    public Needle Needle;

    public void SetHand(Hand hand) {
        Hand = hand;
    }

    public void SetLuerlockPair(KeyValuePair<LuerlockAdapter.Side, LuerlockAdapter> pair) {
        if (pair.Value == null) {
            Logger.Error("Luerlock pair value was null. Problem with SetInteractors cast?");
            return;
        }
        LuerlockPair = pair;
    }

    public void SetBottle(GameObject bottle) {
        Bottle = bottle;
    }

    public void SetNeedle(Needle needle) {
        if (needle == null) {
            Logger.Error("Needle value was null. Problem with SetInteractors cast?");
        }
        this.Needle = needle;
    }
}