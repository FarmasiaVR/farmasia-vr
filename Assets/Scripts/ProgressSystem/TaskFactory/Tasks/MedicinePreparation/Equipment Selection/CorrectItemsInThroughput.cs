using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Correct amount of items inserted into Throughput.
/// </summary>
public class CorrectItemsInThroughput : TaskBase {
    #region Fields
    private string[] conditions = {"BigSyringe", "SmallSyringes", "Needles", "Luerlock", "RightSizeBottle"};
    private int smallSyringes, needles;
    private int objectCount;
    private int checkTimes;
    private PassThroughCabinet cabinet;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for CorrectItemsInThroughput task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public CorrectItemsInThroughput() : base(TaskType.CorrectItemsInThroughput, true, false) {
        Subscribe();
        AddConditions(conditions);
        SetItemsToZero();
        checkTimes = 0;
        points = 2;
        cabinet = GameObject.FindGameObjectWithTag("PassThrough (Prep)")?.GetComponent<PassThroughCabinet>();
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() { 
        base.SubscribeEvent(CorrectItems, EventType.RoomDoor);
    }
    /// <summary>
    /// Once fired by an event, checks which items are in the throughput cabinet and sets the corresponding conditions to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void CorrectItems(CallbackData data) {
        if (data.DataString != DoorGoTo.EnterWorkspace.ToString()) {
            return;
        }
        if (cabinet == null) {
            Logger.Print("There is no Pass-Through Cabinet or Component PassThroughCabinet :S");
            return;
        }
        List<GameObject> containedObjects = cabinet.GetContainedItems();
        if (containedObjects.Count == 0) {
            return;
        }
        checkTimes++;
        objectCount = containedObjects.Count;

        foreach(GameObject value in containedObjects) {
            GeneralItem item = value.GetComponent<GeneralItem>();
            ObjectType type = item.ObjectType;
            switch (type) {
                case ObjectType.Syringe:
                    Syringe syringe = item as Syringe;
                    if (syringe.Container.Capacity == 20) {
                        EnableCondition("Syringe"); 
                    } else if (syringe.Container.Capacity == 1) {
                        smallSyringes++;
                        if (smallSyringes == 6) {
                            EnableCondition("SmallSyringes");
                        }
                    }
                    break;
                case ObjectType.Needle:
                    needles++;
                    if (needles == 7) {
                        EnableCondition("Needles"); 
                    }
                    break;
                case ObjectType.Luerlock:
                    EnableCondition("Luerlock");
                    break;
                case ObjectType.Bottle:
                    MedicineBottle bottle = item as MedicineBottle;
                    if (bottle.Container.Capacity == 100) {
                        EnableCondition("RightSizeBottle");
                    }
                    break;
            }
        }
        
        bool check = CheckClearConditions(true);
        if (!check) {
            if (checkTimes == 1) {
                UISystem.Instance.CreatePopup(-1, "Missing items", MessageType.Mistake);
                G.Instance.Progress.calculator.Subtract(TaskType.CorrectItemsInThroughput);
            }
            smallSyringes = 0;
            needles = 0;
            DisableConditions();
        }
    }
    #endregion

    #region Private Methods
    private void SetItemsToZero() {
        smallSyringes = 0;
        needles = 0;
}
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        if (checkTimes == 1) {
            if (objectCount == 16) {
                UISystem.Instance.CreatePopup(1, "Right amount of items", MessageType.Notify);
            } else {
                UISystem.Instance.CreatePopup(0, "Too many items", MessageType.Notify);
            }
        }
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