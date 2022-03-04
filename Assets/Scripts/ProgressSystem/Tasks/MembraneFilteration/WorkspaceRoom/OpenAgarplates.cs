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

    /// <summary>
    /// When event is fired, checkwhat type of plate was opened and increment amount. If both have been opened end task.
    /// Reduce points if player opens too many plates.
    /// </summary>
    /// <param name="data"></param>
    private void TrackOpenedPlates(CallbackData data) {
        GameObject gameObject = (GameObject)data.DataObject;
        Logger.Print(gameObject);
        ObjectType plateType = gameObject.GetComponent<AgarPlateBottom>().ObjectType;
        if (plateType == ObjectType.SabouradDextrosiPlate) {
            sabouradPlates++;
        }
        if (plateType == ObjectType.SoycaseinePlate) {
            soycaseinePlates++;
        }

        if (soycaseinePlates >= 1 && sabouradPlates >= 1) {
            if (soycaseinePlates + sabouradPlates > 2) {
                CreateTaskMistake("Avasit liian monta maljaa >:(", soycaseinePlates + sabouradPlates - 2);
            }
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