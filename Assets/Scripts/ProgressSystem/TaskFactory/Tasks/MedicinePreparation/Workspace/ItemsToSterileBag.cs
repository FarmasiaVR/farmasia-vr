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
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 2;
        TaskMovedToSide = false;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(PutToBag, EventType.SterileBag);
        base.SubscribeEvent(HandsExit, EventType.HandsExitLaminarCabinet);
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedInCabinet);
        }
    }

    /// <summary>
    /// Once fired by an event, checks how many syringe objects are put to bag object.
    /// Sets corresponding condition to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void PutToBag(CallbackData data2) {

        sterileBag = (SterileBag)data2.DataObject;

        if (!CheckPreviousTaskCompletion(requiredTasks)) {
            UISystem.Instance.CreatePopup("Valmistele aluksi kaikki steriiliin pussiin tulevat ruiskut.", MsgType.Notify);
            G.Instance.Audio.Play(AudioClipType.MistakeMessage);
            return;
        }

        int filledSyringesInCabinet = 0;
        int filledSyringesInBag = 0;
        filledSyringesInCabinet = GetSyringesLiquidCount(GameObjectsToSyringes(laminarCabinet.objectsInsideArea), filledSyringesInCabinet);
        filledSyringesInBag = GetSyringesLiquidCount(sterileBag.Syringes, filledSyringesInBag);

        if (sterileBag.IsClosed) {
            if (filledSyringesInCabinet == filledSyringesInBag) {
                EnableCondition(Conditions.SyringesPut);
            }
            bool check = CheckClearConditions(true);
            if (!check) {
                FinishTask();
            }
        } else {
            if (filledSyringesInCabinet == filledSyringesInBag) {
                UISystem.Instance.CreatePopup("Steriili pussi täynnä ja suljettu.", MsgType.Notify);
            }
        }
    }

    private List<Syringe> GameObjectsToSyringes(List<GameObject> objects) {
        List<Syringe> syringes = new List<Syringe>();

        foreach (GameObject g in objects) {
            Syringe s = g.GetComponent<Syringe>();
            if (s != null) {
                syringes.Add(s);
            }
        }

        return syringes;
    }

    private int GetSyringesLiquidCount(List<Syringe> syringes, int count) {
        foreach (Syringe syringe in syringes) {
            if (syringe.Container.Amount > 0 && !syringe.hasBeenInBottle) {
                count++;
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
        RemoveCapFactories();
    }

    private void RemoveCapFactories() {
        GameObject[] gobjs = GameObject.FindGameObjectsWithTag("CapFactory");
        foreach (GameObject obj in gobjs) {
            GameObject.Destroy(obj);
        }
    }

    private bool CapsOnSyringes() {
        int count = 0;
        foreach (Syringe syringe in sterileBag.Syringes) {
            if (syringe.HasSyringeCap) {
                count++;
            }
        }
        if (count == sterileBag.Syringes.Count) {
            return true;
        }
        return false;
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        if (sterileBag.Syringes.Count >= 6) {
            if (CapsOnSyringes()) {
                if (!TaskMovedToSide) {
                    UISystem.Instance.CreatePopup("Ruiskut laitettiin steriiliin pussiin.", MsgType.Done);
                }
            } else {
                if (!TaskMovedToSide) {
                    UISystem.Instance.CreatePopup(1, "Pakattiin oikea määrä ruiskuja mutta kaikissa ei ollut korkkia.", MsgType.Mistake);
                }
                G.Instance.Progress.Calculator.Subtract(TaskType.ItemsToSterileBag);
            }
        } else {
            if (sterileBag.Syringes.Count > 0 && CapsOnSyringes()) {
                if (!TaskMovedToSide) {
                    UISystem.Instance.CreatePopup(1, "Kaikkia täytettyjä ruiskuja ei laitettu steriiliin pussiin.", MsgType.Mistake);
                }
                G.Instance.Progress.Calculator.Subtract(TaskType.ItemsToSterileBag);
            } else {
                if (!TaskMovedToSide) {
                    UISystem.Instance.CreatePopup(0, "Kaikkia täytettyjä ruiskuja ei laitettu steriiliin pussiin ja kaikissa pakatuissa ruiskuissa ei ollut korkkia.", MsgType.Mistake);
                }
                G.Instance.Progress.Calculator.SubtractWithScore(TaskType.ItemsToSterileBag, points);
            }
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