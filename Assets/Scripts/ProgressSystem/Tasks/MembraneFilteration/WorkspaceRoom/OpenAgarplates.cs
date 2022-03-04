using UnityEngine;
using System;
using System.Collections.Generic;

public class OpenAgarplates : TaskBase {

    #region Constants
    private const string DESCRIPTION = "Avaa laskeumamaljat";
    private const string HINT = "Avaa yksi soijakaseiinimalja sekä yksi sabouradekstrosimalja";
    #endregion

    #region Fields
    /// <summary>
    /// Conditions must be met to render task complete
    /// </summary>
    public enum Conditions { OpenedCorrectPlates }
    private int soycaseinePlates = 0;
    private int sabouradPlates = 0;
    #endregion

    public OpenAgarplates() : base(TaskType.OpenAgarplates, true, true) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 2;
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackOpenedPlates, EventType.PlateOpened);
    }

    private void TrackOpenedPlates(CallbackData data) {
        Logger.Print("Tracking opened plates");
        GameObject gameObject = (GameObject)data.DataObject;
        ObjectType plateType = gameObject.GetComponent<AgarPlateLid>().ObjectType;
        if (plateType == ObjectType.SabouradDextrosiPlate) {
            sabouradPlates++;
        }
        if (plateType == ObjectType.SoycaseinePlate) {
            soycaseinePlates++;
        }

        if (soycaseinePlates + sabouradPlates == 4) {
            EnableCondition(Conditions.OpenedCorrectPlates);
            CompleteTask();
        }
    }
    protected override void OnTaskComplete() {
    }

    public override void CompleteTask() {
        base.CompleteTask();
        if (IsCompleted()) {
            Popup("Hienosti avattu!", MsgType.Done);
        }
    }

    public override string GetDescription() {
        return DESCRIPTION;
    }

    public override string GetHint() {
        return HINT;
    }
}