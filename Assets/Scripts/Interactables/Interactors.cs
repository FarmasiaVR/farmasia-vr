using System.Collections.Generic;
using UnityEngine;

public struct Interactors {
    public Hand Hand;
    public KeyValuePair<int, LuerlockAdapter> LuerlockPair;
    public GameObject Bottle;

    public void SetHand(Hand hand) {
        Hand = hand;
    }
    public void SetLuerlockPair(KeyValuePair<int, LuerlockAdapter> pair) {
        LuerlockPair = pair;
    }
    public void SetBottle(GameObject bottle) {
        Bottle = bottle;
    }
}