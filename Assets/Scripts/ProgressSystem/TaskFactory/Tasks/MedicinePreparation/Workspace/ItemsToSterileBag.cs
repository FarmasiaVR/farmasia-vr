using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsToSterileBag : TaskBase {

    #region Constants
    private const string DESCRIPTION = "Viimeistele ruiskujen kanssa työskentely.";
    private const string HINT = "Laita täyttämäsi ruiskut steriiliin pussiin.";
    #endregion

    #region Fields
    public enum Conditions { SyringesPut }
    private List<TaskType> requiredTasks = new List<TaskType> { TaskType.CorrectAmountOfMedicineSelected };
    private CabinetBase laminarCabinet;
    bool TaskMovedToSide;
    private SterileBag sterileBag;
    
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for ItemsToSterileBag task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public ItemsToSterileBag() : base(TaskType.ItemsToSterileBag, true, false) {
        Subscribe();
        AddConditions((int[]) Enum.GetValues(typeof(Conditions)));
        points = 2;
        TaskMovedToSide = false;
    }
    #endregion

    #region Event Subscriptions
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
            AudioManager.Instance?.Play("mistakeMessage");
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
                G.Instance.Progress.Calculator.SubtractWithScore(TaskType.ItemsToSterileBag, points);
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
        if (CheckPreviousTaskCompletion(requiredTasks) && !TaskMovedToSide) {
            MoveTaskToSide();
        }
    }

    private void MoveTaskToSide() {
        G.Instance.Progress.ChangePackage();
        G.Instance.Progress.UpdateDescription();
        TaskMovedToSide = true;
    }

    private bool CapsOnSyringes() {
        int count = 0;
        foreach(GameObject value in sterileBag.objectsInBag) {
            GeneralItem item = value.GetComponent<GeneralItem>();
            ObjectType type = item.ObjectType;
            if (type == ObjectType.Syringe) {
                Syringe syringe = item as Syringe;
                if (syringe.HasSyringeCap) {
                    count++;
                }
            }
        }
        if (count == 6) {
            return true; 
        }
        return false;
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        if (CapsOnSyringes()) {
            UISystem.Instance.CreatePopup("Ruiskut laitettiin steriiliin pussiin.", MsgType.Done);
        } else {
            UISystem.Instance.CreatePopup(1, "Kaikissa ruiskuissa ei ollut korkkia.", MsgType.Mistake);
            G.Instance.Progress.Calculator.Subtract(TaskType.ItemsToSterileBag);
        }
        
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