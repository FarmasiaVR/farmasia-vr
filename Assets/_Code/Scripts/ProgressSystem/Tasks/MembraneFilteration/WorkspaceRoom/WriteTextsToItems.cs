using UnityEngine;
using System;
using System.Collections.Generic;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class WriteTextsToItems : Task {

    #region Fields
    /// <summary>
    /// Conditions must be met to render task complete
    /// </summary>
    public enum Conditions {
        ObjectsHaveText
    }
    private int numberOfObjectsThatShouldHaveText = 8;
    private List<GameObject> writtenObjects;
    #endregion

    public WriteTextsToItems() : base(TaskType.WriteTextsToItems, false) {
        SetCheckAll(true);
        
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        writtenObjects = new List<GameObject>();
    }

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(TrackWrittenObjects, EventType.WriteToObject);
    }

    /// <summary>
    /// Keep list of game objects that the player has written text
    /// Check if game object has previously been written on, if yes, replace with the current (callback object) one.
    /// If game object is not on the list, add it and increment the type counters.
    /// </summary>
    /// <param name="data"> gameObject that the player has written text on</param>
    private void TrackWrittenObjects(CallbackData data) {
        GameObject gameObject = (GameObject)data.DataObject;

        bool containsObject = false;
        int index = 0;
        foreach (var gObject in writtenObjects) {
            if (gObject.GetInstanceID() == gameObject.GetInstanceID()) {
                index = writtenObjects.IndexOf(gObject);
                containsObject = true;
            }
        }

        if (containsObject) {
            writtenObjects[index] = gameObject;
        } else {
            writtenObjects.Add(gameObject);
        }

        if (writtenObjects.Count == numberOfObjectsThatShouldHaveText) {
            EnableCondition(Conditions.ObjectsHaveText);
            CheckWrittenObjects();
            CompleteTask();
        }

    }
    #endregion

    /// <summary>
    /// Check if items have correct writing on them. Whenever matching requirement is found, item is removed from writtenObjects list.
    /// If after first loop tere are still items, it means that those have incorrect text on them. 
    /// They are then iterated again, this time with checkForMistake flag turned on that creates task mistakes.
    /// </summary>
    public void CheckWrittenObjects() {
        List<WritingSpec> writingSpecs = WritingSpecifications.GetInitialRequiredWritings();

        for (int i = writtenObjects.Count - 1; i >= 0; i--) {
            var gameObject = writtenObjects[i];
            if (Matches(gameObject, writingSpecs, false)) {
                writtenObjects.RemoveAt(i);
            }
            if (gameObject.GetComponent<GeneralItem>().ObjectType == ObjectType.SoycaseinePlate || gameObject.GetComponent<GeneralItem>().ObjectType == ObjectType.SabouradDextrosiPlate) {
                gameObject.GetComponent<Writable>().MaxLines += 1;
            }
        }

        if (writtenObjects.Count > 0) {
            for (int i = writtenObjects.Count - 1; i >= 0; i--) {
                var gameObject = writtenObjects[i];
                Matches(gameObject, writingSpecs, true);
            }
        }
    }

    /// <summary>
    /// Iterate gameObject through writing requirements. If matching requirement is found, method removes that specific requirement from
    /// writingSpecs and returns true. gameObject is then removed in CheckWrittenObjects.
    /// </summary>
    /// <param name="gameObject"> Game object to be checked for mistakes</param>
    /// <param name="writingSpecs"> Specifications about wanted information eg. date, name etc</param>
    /// <param name="checkForMistakes"> true if we want to create task mistakes, false otherwise</param>
    /// <returns></returns>
    public bool Matches(GameObject gameObject, List<WritingSpec> writingSpecs, bool checkForMistakes) {
        GeneralItem item = gameObject.GetComponent<GeneralItem>();
        Writable writable = gameObject.GetComponent<Writable>();
        bool correctObjectType = false;
        if (item == null || writable == null)
            return false;
        for (int i = writingSpecs.Count - 1; i >= 0; i--) {
            var writingSpec = writingSpecs[i];
            if (writingSpec.objectType == item.ObjectType) {
                correctObjectType = true;
                bool foundAllRequiredWritings = true;
                WritingType[] requiredWritings = writingSpec.requiredWritings;
                foreach (var requiredWriting in requiredWritings) {
                    if (!writable.WrittenLines.ContainsKey(requiredWriting)) {
                        foundAllRequiredWritings = false;
                        if (checkForMistakes)
                            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "MissingWritings1") + requiredWriting + Translator.Translate("XR MembraneFilteration 2.0", "MissingWritings2") + item.ObjectType, 1);
                    }
                }
                if (foundAllRequiredWritings) {
                    int index = writingSpecs.IndexOf(writingSpec);
                    writingSpecs.RemoveAt(index);
                    ;
                    return true;
                }
            }
        }
        if (correctObjectType == false && checkForMistakes) {
            CreateTaskMistake(Translator.Translate("XR MembraneFilteration 2.0", "WritingInWrongItem"), 1);
        }
        return false;
    }
}