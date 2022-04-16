using UnityEngine;
using System;
public class SelectMedicine : Task {

    #region Fields
    public enum Conditions { BottlePickup }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor for SelectMedicine task. 
    ///  Is removed when finished and doesn't require previous task completion.
    /// </summary>
    public SelectMedicine() : base(TaskType.SelectMedicine, false) {
        SetCheckAll(true);
        
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(PickupObject, EventType.PickupObject);
    }

    /// <summary>
    /// Once fired by an event, checks if the item is bottle and sets the corresponding condition to be true.
    /// </summary>
    /// <param name="data">Refers to the data returned by the trigger.</param>
    private void PickupObject(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (type == ObjectType.Bottle || type == ObjectType.Medicine) {
            EnableCondition(Conditions.BottlePickup);
        }
        CompleteTask();
    }
    #endregion

    #region Public Methods
    
    public override void FinishTask() {
        base.FinishTask();
        
    }
    #endregion
}