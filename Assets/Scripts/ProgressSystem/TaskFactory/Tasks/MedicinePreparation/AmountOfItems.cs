using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmountOfItems : TaskBase {
    #region Fields
    private string[] conditions = {"Syringe", "Needle", "Luerlock", "RightSizeBottle"};
    #endregion

    #region Constructor
    public AmountOfItems() : base(TaskType.AmountOfItems, true, false) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() { 
        base.SubscribeEvent(Amount, EventType.AmountOfItems);
    }
    private void Amount(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        
        switch (type) {
            case ObjectType.Syringe:
                EnableCondition("Syringe");
                break;
            case ObjectType.Needle:
                EnableCondition("Needle");
                break;
            case ObjectType.Luerlock:
                EnableCondition("Luerlock");
                break;
            case ObjectType.Bottle:
                //check that the chosen bottle is the wanted size
                EnableCondition("RightSizeBottle");
                break;
        }
        
        bool check = CheckClearConditions(true);
        //checked when player exits the room
        if (!check) {
            UISystem.Instance.CreatePopup(-1, "Wrong amount of items", MessageType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.AmountOfItems);
            base.FinishTask();
            G.Instance.Progress.AddTask(TaskFactory.GetTask(TaskType.MissingItems));
        }
    } 
    #endregion

    #region Public Methods
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Right amount of items", MessageType.Notify);
        G.Instance.Progress.Calculator.Add(TaskType.AmountOfItems);
        base.FinishTask();
    }
    public override string GetDescription() {
        return "Tarkista valitsemiesi välineiden määrä.";
    }
    public override string GetHint() {
        return "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla."; 
    }
    #endregion
}