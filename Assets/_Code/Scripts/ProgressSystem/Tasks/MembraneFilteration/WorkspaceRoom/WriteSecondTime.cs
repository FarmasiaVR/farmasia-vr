using UnityEngine;
using System;
using System.Collections.Generic;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class WriteSecondTime : Task {

    #region Fields
    /// <summary>
    /// Conditions must be met to render task complete
    /// </summary>
    public enum Conditions {
        TimesWritten
    }
    // private int numberOfObjectsThatShouldHaveText = 4;
    private List<GameObject> writtenObjects;
    private bool sabourad = false;
    private bool soycaseine = false;
    #endregion

    public WriteSecondTime() : base(TaskType.WriteSecondTime, false) {
        SetCheckAll(true);

        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        writtenObjects = new List<GameObject>();
    }

    public override void Subscribe() {
        base.SubscribeEvent(TrackWrittenObjects, EventType.WriteToObject);
    }

    private void TrackWrittenObjects(CallbackData data) {
        GameObject gameObject = (GameObject)data.DataObject;
        GeneralItem item = gameObject.GetComponent<GeneralItem>();
        Writable writable = gameObject.GetComponent<Writable>();
        if (Started) {

            if (item.ObjectType == ObjectType.SabouradDextrosiPlate) {
                
                if (writable.WrittenLines.ContainsKey(WritingType.SecondTime) || writable.WrittenLines.ContainsKey(WritingType.SecondFakeTime)){
                    sabourad = true;
                }
                
                if(writable.WrittenLines.ContainsKey(WritingType.SecondFakeTime))
                {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongTimeInAgarPlate"), 1);
                }
            }
            if (item.ObjectType == ObjectType.SoycaseinePlate) {
                if (writable.WrittenLines.ContainsKey(WritingType.LeftHand) || writable.WrittenLines.ContainsKey(WritingType.RightHand)) {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongWritingInAgarPlate"), 1);
                } else if (writable.WrittenLines.ContainsKey(WritingType.SecondTime) || writable.WrittenLines.ContainsKey(WritingType.SecondFakeTime)) {
                    soycaseine = true;
                }

                if (writable.WrittenLines.ContainsKey(WritingType.SecondFakeTime))
                {
                    CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WrongTimeInAgarPlate"), 1);
                }

            }
            if (sabourad && soycaseine) {
                EnableCondition(Conditions.TimesWritten);
                CompleteTask();
            }
        } else {
            return;
        }

    }
}