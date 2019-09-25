using UnityEngine;

public class AmountOfItems : TaskBase {
    #region Fields
    private string[] conditions = {"Syringe", "Needle", "Luerlock", "RightSizeBottle"};
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for AmountOfItems task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public AmountOfItems() : base(TaskType.AmountOfItems, true, false) {
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() { 
        base.SubscribeEvent(Amount, EventType.AmountOfItems);
    }
    /// <summary>
    /// Once fired by and event, checks which item was picked and sets the corresponding condition to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
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
                //check that the chosen bottle has the wanted size
                EnableCondition("RightSizeBottle");
                break;
        }
        
        bool check = CheckClearConditions(true);
        //checked when player exits the room
        if (!check) {
            UISystem.Instance.CreatePopup(-1, "Wrong amount of items", MessageType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.AmountOfItems);
            base.FinishTask();

            MissingItems missingTask = TaskFactory.GetTask(TaskType.MissingItems) as MissingItems;
            missingTask.SetMissingConditions(GetNonClearedConditions());
            G.Instance.Progress.AddTask(missingTask);
        }
    } 
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup(1, "Right amount of items", MessageType.Notify);
        G.Instance.Progress.Calculator.Add(TaskType.AmountOfItems);
        base.FinishTask();
    }
    
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "Tarkista valitsemiesi välineiden määrä.";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla."; 
    }
    #endregion
}