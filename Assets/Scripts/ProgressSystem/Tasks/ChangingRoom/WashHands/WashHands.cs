using UnityEngine;
using System;
public class WashHands : Task {

    #region Fields
    public enum Conditions { SoapUsed }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor for WashHands task. 
    /// x.
    /// </summary>
    public WashHands() : base(TaskType.WashHands, false) {
        SetCheckAll(false);

        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(HandsTouched, EventType.WashHandsSoapUsed);
    }

    // At Start hands = Dirty, If Soap touched --> hands = Soapy,
    // if Water touched when hands == Soapy --> Hands = wet, if Disinfectant touched when hands == Soapy --> hands = clean


    // From SelectMedicine

    /// <summary>
    /// Once fired by an event, checks if the touched item is SoapDispencer(Collider) and sets the corresponding condition to be true.
    /// </summary>
    /// <param name="data">Refers to the data returned by the trigger.</param>
    private void HandsTouched(CallbackData data) {

        
        GameObject g = data.DataObject as GameObject;
        SoapDispencer item = g.GetComponent<SoapDispencer>();
        if (item == null) {
            return;
        }
        

        ObjectType type = item.ObjectType;
        if (type == ObjectType.SoapDispencer) {
            EnableCondition(Conditions.SoapUsed);
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