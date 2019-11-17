using UnityEngine;
using System;
public class SelectTools : TaskBase {

    #region Constants
    private const string DESCRIPTION = "Valitse sopivat työvälineet.";
    private const string HINT = "Huoneessa on lääkkeen valmistukseen tarvittavia työvälineitä. Valitse oikea määrä ruiskuja, neuloja ja luerlockeja.";
    #endregion

    #region Fields
    public enum Conditions { SyringePickedUp, NeedlePickedUp, LuerlockPickedUp }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor for SelectTools task. 
    /// Is removed when finished and doesn't require previous task completion.
    /// </summary>
    public SelectTools() : base(TaskType.SelectTools, true, false) {
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(PickupObject, EventType.PickupObject);
    }

    /// <summary>
    /// Once fired by an event, checks if the item is correct and sets corresponding condition to be true.
    /// </summary>
    /// <param name="data">Refers to the GameObject that was picked up.</param>
    private void PickupObject(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        switch (type) {
            case ObjectType.Syringe:
                EnableCondition(Conditions.SyringePickedUp);
                break;
            case ObjectType.Needle:
                EnableCondition(Conditions.NeedlePickedUp);
                break;
            case ObjectType.Luerlock:
                EnableCondition(Conditions.LuerlockPickedUp);
                break;
        }
        CheckClearConditions(false);
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        UISystem.Instance.CreatePopup("Työväline valittu.", MsgType.Done);
        base.FinishTask();
    }

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        return HINT;
    }
    #endregion
}