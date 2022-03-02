using System.Collections.Generic;
using UnityEngine;

public struct Interactors {

    public Hand Hand;
    public KeyValuePair<LuerlockAdapter.Side, LuerlockAdapter> LuerlockPair { get; private set; }
    public GameObject Bottle;
    public Needle Needle { get; private set; }
    public AgarPlateLid AgarPlateLid { get; private set;  }
    public PumpFilter PumpFilter { get; private set; }

    public void SetHand(Hand hand) {
        Hand = hand;
    }

    public void SetAgarPlateLid(AgarPlateLid lid) {
        AgarPlateLid = lid;
    }

    public void ResetAgarPlateLid() {
        AgarPlateLid = null;
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

    public void SetBottle(GameObject bottle) {
        Bottle = bottle;
    }

    public void SetNeedle(Needle needle) {
        if (needle == null) {
            Logger.Error("Needle value was null. Problem with SetInteractors cast?");
        }
        this.Needle = needle;
    }
    public void ResetNeedle() {
        this.Needle = null;
    }
    public void SetPumpFilter(PumpFilter filter)
    {
        if (filter == null)
        {
            Logger.Error("PumpFilter value was null. Problem with SetInteractors cast?");
        }
        this.PumpFilter = filter;
    }
    public void ResetPumpFilter()
    {
        this.PumpFilter = null;
    }



}