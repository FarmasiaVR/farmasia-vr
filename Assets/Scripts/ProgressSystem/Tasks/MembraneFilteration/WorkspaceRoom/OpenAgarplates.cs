using UnityEngine;
using System;
using System.Collections.Generic;

public class OpenAgarplates : Task {

    /*#region Constants
    public override string Description { get => "Avaa laskeumamaljat"; }
    private const string HINT = "Avaa yksi soijakaseiinimalja sek� yksi sabouradekstrosimalja";
    #endregion*/

    #region Fields
    /// <summary>
    /// Conditions must be met to render task complete
    /// </summary>
    public enum Conditions { OpenedCorrectPlates }
    private int soycaseinePlates = 0;
    private int sabouradPlates = 0;
    private CabinetBase laminarCabinet;
    #endregion

    public OpenAgarplates() : base(TaskType.OpenAgarplates, true) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackOpenedPlates, EventType.PlateOpened);
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }
    /// <summary>
    /// When event is fired, checkwhat type of plate was opened and increment amount. If both have been opened end task.
    /// Reduce points if player opens too many plates.
    /// </summary>
    /// <param name="data"></param>
    private void TrackOpenedPlates(CallbackData data) {
        var lid = (data.DataObject as AgarPlateLid);
        if (lid == null) return;

        if (laminarCabinet == null || !laminarCabinet.GetContainedItems().Contains(lid)) {
            CreateTaskMistake("Avasit maljan laminaarikaapin ulkopuolella!!!", 1);
            return;
        }

        if (lid.Variant == "Soija-kaseiini") {
            sabouradPlates++;
        }
        if (lid.Variant == "Sabourad-dekstrosi") {
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

    public override void CompleteTask() {
        base.CompleteTask();
        if (Completed) {
            Popup(base.success, MsgType.Done, base.Points);
        }
    }
}