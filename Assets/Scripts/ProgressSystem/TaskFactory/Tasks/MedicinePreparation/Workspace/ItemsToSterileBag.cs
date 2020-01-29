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

    private const int correctCapacity = 1000;
    private const int correctAmount = 150;

    private enum SterileBagMistake {
        IncorrectSyringe,
        NoCap,
        IncorrectAmountOfMedicine,
        ContaminatedSyringe
    }
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for ItemsToSterileBag task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public ItemsToSterileBag() : base(TaskType.ItemsToSterileBag, true, false) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 2;
        TaskMovedToSide = false;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(PutToBag, EventType.SterileBag);
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

        if (!IsPreviousTasksCompleted(requiredTasks)) {

            Popup("Valmistele aluksi kaikki steriiliin pussiin tulevat ruiskut.", MsgType.Notify);
            return;
        }

        int filledSyringesInCabinet = 0;
        int filledSyringesInBag = 0;
        filledSyringesInCabinet = GetSyringesLiquidCount(GameObjectsToSyringes(laminarCabinet.objectsInsideArea), filledSyringesInCabinet);
        filledSyringesInBag = GetSyringesLiquidCount(sterileBag.Syringes, filledSyringesInBag);

        if (!sterileBag.IsClosed) {
            if (filledSyringesInCabinet == filledSyringesInBag) {
                Logger.Print(filledSyringesInCabinet + " : " + filledSyringesInBag);
                Logger.Print("ENABLING SYRINGESPUT CONDITION");
                EnableCondition(Conditions.SyringesPut);
            }
            CompleteTask();

            if (IsCompleted()) {
                FinishTask();
            }
        } else {
            if (filledSyringesInCabinet == filledSyringesInBag) {
                Popup("Steriili pussi täynnä ja suljettu.", MsgType.Notify);
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
    #endregion

    #region Public Methods
    public override void FinishTask() {
        if (!isFinished) {
            //if (sterileBag.Syringes.Count >= 6) {
            //    if (CapsOnSyringes()) {
            //        if (TaskMovedToSide) {
            //            Popup("Ruiskut laitettiin steriiliin pussiin.", MsgType.Done, 2);
            //        }
            //    } else {
            //        if (TaskMovedToSide) {
            //            Popup("Pakattiin oikea määrä ruiskuja mutta kaikissa ei ollut korkkia.", MsgType.Mistake, -1);
            //        }
            //        G.Instance.Progress.Calculator.Subtract(TaskType.ItemsToSterileBag);
            //    }
            //} else {
            //    if (sterileBag.Syringes.Count > 0 && CapsOnSyringes()) {
            //        if (TaskMovedToSide) {
            //            Popup("Kaikkia täytettyjä ruiskuja ei laitettu steriiliin pussiin.", MsgType.Mistake, -1);
            //        }
            //        G.Instance.Progress.Calculator.Subtract(TaskType.ItemsToSterileBag);
            //    } else {
            //        if (TaskMovedToSide) {
            //            Popup("Kaikkia täytettyjä ruiskuja ei laitettu steriiliin pussiin ja kaikissa pakatuissa ruiskuissa ei ollut korkkia.", MsgType.Mistake, -2);
            //        }
            //        G.Instance.Progress.Calculator.SubtractWithScore(TaskType.ItemsToSterileBag, 2);
            //    }
            //}

            int mistakes = 0;
            HashSet<SterileBagMistake> mistakeList = new HashSet<SterileBagMistake>();
            foreach (Syringe syringe in sterileBag.Syringes) {
                CheckSyringe(syringe, ref mistakes, ref mistakeList);
            }

            string errorString = "Virheet: ";
            foreach (SterileBagMistake m in mistakeList) {
                errorString += MistakeToString(m);
            }

            if (mistakes > 0) {
                Popup(errorString, MsgType.Mistake);
                G.Instance.Progress.Calculator.SubtractWithScore(TaskType.ItemsToSterileBag, mistakes);
            } else {
                if (TaskMovedToSide) {
                    Popup("Ruiskut laitettiin steriiliin pussiin.", MsgType.Done);
                }
            }

            base.FinishTask();
        }
    }
    private string MistakeToString(SterileBagMistake mistake) {
        switch (mistake) {
            case SterileBagMistake.IncorrectSyringe:
                return "väärän kokoinen ruisku, ";
            case SterileBagMistake.NoCap:
                return "ei korkkia, ";
            case SterileBagMistake.IncorrectAmountOfMedicine:
                return "väärä määrä lääkettä, ";
            case SterileBagMistake.ContaminatedSyringe:
                return "ruisku oli likainen, ";
        }
        return "";
    }

    private void CheckSyringe(Syringe syringe, ref int mistakes, ref HashSet<SterileBagMistake> mistakesList) {
        if (syringe.Container.Capacity != correctCapacity) {
            mistakes--;
            mistakesList.Add(SterileBagMistake.IncorrectSyringe);
        }

        if (!syringe.HasSyringeCap) {
            mistakes--;
            mistakesList.Add(SterileBagMistake.NoCap);
        }

        if (syringe.Container.Amount != correctAmount) {
            mistakes--;
            mistakesList.Add(SterileBagMistake.IncorrectAmountOfMedicine);
        }

        if (!syringe.IsClean) {
            mistakes--;
            mistakesList.Add(SterileBagMistake.ContaminatedSyringe);
        }
    }

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        return HINT;
    }

    protected override void OnTaskComplete() {
        Logger.Print("DISABLING CAP FACTORY");
        laminarCabinet.DisableCapFactory();
    }
    #endregion
}