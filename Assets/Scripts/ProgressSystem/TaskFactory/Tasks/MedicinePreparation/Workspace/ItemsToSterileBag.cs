using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsToSterileBag : TaskBase {

    #region Fields
    public enum Conditions { SyringesPut }
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.CorrectAmountOfMedicineSelected };
    private CabinetBase laminarCabinet;
    private SterileBag sterileBag;
    private string description = "Viimeistele ruiskujen kanssa työskentely.";
    private string hint = "Laita täyttämäsi ruiskut steriiliin pussiin.";
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for ItemsToSterileBag task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public ItemsToSterileBag() : base(TaskType.ItemsToSterileBag, true, false) {
        Subscribe();
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        points = 1;
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(PutToBag, EventType.SterileBag);
        base.SubscribeEvent(HandsExit, EventType.HandsExitLaminarCabinet);
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        base.SubscribeEvent(SetSterileBagReference, EventType.ItemPlacedInSterileBag);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        }        
    }

    private void SetSterileBagReference(CallbackData data) {
        sterileBag = (SterileBag)data.DataObject;
        base.UnsubscribeEvent(SetSterileBagReference, EventType.ItemPlacedInSterileBag);        
    }
    
    /// <summary>
    /// Once fired by an event, checks how many syringe objects are put to bag object.
    /// Sets corresponding condition to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void PutToBag(CallbackData data) {
        if (!CheckPreviousTaskCompletion(requiredTasks)) {
            UISystem.Instance.CreatePopup("Valmistele aluksi kaikki steriiliin pussiin tulevat ruiskut.", MsgType.Notify);
            return;
        }
        List<GameObject> inBag = data.DataObject as List<GameObject>;
        int filledSyringesInCabinet = 0;
        int filledSyringesInBag = 0;
        filledSyringesInCabinet = CheckFilledSyringes(laminarCabinet.objectsInsideArea, filledSyringesInCabinet);
        filledSyringesInBag = CheckFilledSyringes(inBag, filledSyringesInBag);
        
        if (sterileBag.IsClosed) {
            if (filledSyringesInCabinet == filledSyringesInBag) {
                EnableCondition(Conditions.SyringesPut);
            }
            bool check = CheckClearConditions(true);
            if (!check) {
                UISystem.Instance.CreatePopup(0, "Kaikkia täytettyjä ruiskuja ei pakattu steriiliin pussiin.", MsgType.Mistake);
                G.Instance.Progress.Calculator.Subtract(TaskType.ItemsToSterileBag);
                base.FinishTask();
            }
        } else {
            if (filledSyringesInCabinet == filledSyringesInBag) {
                UISystem.Instance.CreatePopup("Sulje steriili pussi.", MsgType.Notify);
            }
        } 
    }

    private int CheckFilledSyringes(List<GameObject> objects, int count) {
        foreach(GameObject value in objects) {
            GeneralItem item = value.GetComponent<GeneralItem>();
            ObjectType type = item.ObjectType;
            if (type == ObjectType.Syringe) {
                Syringe syringe = item as Syringe;
                if (syringe.Container.Amount > 0 && !syringe.hasBeenInBottle) {
                    count++;
                }
            }
        }  
        return count; 
    }

    private void HandsExit(CallbackData data) {
        if (CheckPreviousTaskCompletion(requiredTasks)) {
            G.Instance.Progress.ChangePackage();
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup("Ruiskut laitettiin steriiliin pussiin.", MsgType.Done);
        base.FinishTask();
    }
    
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return description;
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return hint;
    }
    #endregion
}