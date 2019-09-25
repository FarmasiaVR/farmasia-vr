using System;
using UnityEngine;

public class MissingItems : TaskBase {
    #region Fields
    private string[] conditions;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for MissingItems task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public MissingItems() : base(TaskType.MissingItems, true, false) {
        conditions = new string[0];
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() { 
        base.SubscribeEvent(Missing, EventType.Missing);
    }
    /// <summary>
    /// Once fired by and event, checks which items have been picked up.
    /// Sets corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void Missing(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        
        //how to check specific missing items??
        if (type == ObjectType.Syringe && Array.Exists(conditions, element => element == "Syringe")) {
            EnableCondition("Syringe");
        }
        if (type == ObjectType.Needle && Array.Exists(conditions, element => element == "Needle")) {
            EnableCondition("Needle");
        }
        if (type == ObjectType.Luerlock && Array.Exists(conditions, element => element == "Luerlock")) {
            EnableCondition("Luerlock");
        }
        if (type == ObjectType.Bottle && Array.Exists(conditions, element => element == "RightSizeBottle")) {
            //check that the chosen bottle has the wanted size
            EnableCondition("RightSizeBottle");
        }
        CheckClearConditions(true);
    }
    #endregion 

    #region Public Methods
    public void SetMissingConditions(string[] missingConditions) {
        conditions = missingConditions;
    }

    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup("Missing items picked up", MessageType.Done);
        base.FinishTask();
    }

    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "Hae puuttuvat työvälineet.";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "Ensimmäisellä hakukerralla osa tarvittavista työvälineistä jäi valitsematta. Palaa ensimmäiseen huoneeseen hakemaan puuttuvat tavarat."; 
    }
    #endregion
}