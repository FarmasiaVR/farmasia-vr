using UnityEngine;
using System;
public class SelectTools : TaskBase {
    #region Fields
    public enum Conditions { SyringePickedUp, NeedlePickedUp, LuerlockPickedUp } 
    private string description = "Valitse sopivat työvälineet.";
    private string hint = "Huoneessa on lääkkeen valmistukseen tarvittavia työvälineitä. Valitse oikea määrä ruiskuja, neuloja ja luerlockeja.";
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor for SelectTools task. 
    /// Is removed when finished and doesn't require previous task completion.
    /// </summary>
    public SelectTools() : base(TaskType.SelectTools, true, false) {
        Subscribe();
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
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
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup("Työväline valittu.", MessageType.Done);
        base.FinishTask();
    }

    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>Returns a String presentation of the description.</returns>
    public override string GetDescription() {
        return description;
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>Returns a String presentation of the hint.</returns>
    public override string GetHint() {
        return hint;
    }
    #endregion
}