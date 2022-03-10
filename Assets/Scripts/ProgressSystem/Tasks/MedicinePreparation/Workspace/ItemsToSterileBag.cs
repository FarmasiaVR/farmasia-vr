using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsToSterileBag : Task {

    #region Constants
    private const string DESCRIPTION = "Viimeistele ruiskujen kanssa työskentely.";
    private const string HINT = "Laita täyttämäsi ruiskut steriiliin pussiin.";
    #endregion

    #region Fields
    public enum Conditions { }
    private CabinetBase laminarCabinet;
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
    public ItemsToSterileBag() : base(TaskType.ItemsToSterileBag, true, false) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 2;
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(OnSterileBagClose, EventType.CloseSterileBag);
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    private void OnSterileBagClose(CallbackData data2) {
        sterileBag = (SterileBag)data2.DataObject;
        G.Instance.Progress.ForceCloseTasks(this);
        CompleteTask();
        FinishTask();
    }
    #endregion

    #region Public Methods
    public override void FinishTask() {
        if (!isFinished) {
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
                CreateTaskMistake(errorString, mistakes);
            } else {
                Popup("Ruiskut laitettiin steriiliin pussiin.", MsgType.Done);
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
            mistakes++;
            mistakesList.Add(SterileBagMistake.IncorrectSyringe);
        }

        if (!syringe.HasSyringeCap) {
            mistakes++;
            mistakesList.Add(SterileBagMistake.NoCap);
        }

        if (syringe.Container.Amount != correctAmount) {
            mistakes++;
            mistakesList.Add(SterileBagMistake.IncorrectAmountOfMedicine);
        }

        if (!syringe.IsClean) {
            mistakes++;
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
    }
    #endregion
}