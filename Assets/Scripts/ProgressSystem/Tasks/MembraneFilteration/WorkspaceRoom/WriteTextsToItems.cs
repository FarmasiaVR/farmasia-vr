using UnityEngine;
using System;
using System.Collections.Generic;
public class WriteTextsToItems : Task
{
    // Instead, use WritingSpecifications.GetInitialRequiredWritings()

    #region Constants
    public new string Description = "Kirjoita tarvittavat tiedot pulloihin ja maljoihin";
    private const string HINT = "Kosketa kyn�ll� esinett�, johon haluat kirjoittaa, valitse kirjoitettavat tekstit (max 4) klikkaamalla niit�. Voit perua kirjoituksen painamalla teksti� uudestaan ennen kuin painat vihre�� nappia";
    #endregion

    #region Fields
    /// <summary>
    /// Conditions must be met to render task complete
    /// </summary>
    public enum Conditions { ObjectsHaveText }
    private int numberOfObjectsThatShouldHaveText = 8;
    private List<GameObject> writtenObjects;
    #endregion

    public WriteTextsToItems() : base(TaskType.WriteTextsToItems, true, false)
    {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        writtenObjects = new List<GameObject>();
        Points = 2;
    }

    #region Event Subscriptions
    public override void Subscribe()
    {
        base.SubscribeEvent(TrackWrittenObjects, EventType.WriteToObject);
    }

    /// <summary>
    /// Keep list of game objects that the player has written text
    /// Check if game object has previously been written on, if yes, replace with the current (callback object) one.
    /// If game object is not on the list, add it and increment the type counters.
    /// </summary>
    /// <param name="data"> gameObject that the player has written text on</param>
    private void TrackWrittenObjects(CallbackData data)
    {
        GameObject gameObject = (GameObject)data.DataObject;

        bool containsObject = false;
        int index = 0;
        foreach (var gObject in writtenObjects)
        {
            if (gObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                index = writtenObjects.IndexOf(gObject);
                containsObject = true;
            }
        }
        
        if (containsObject)
        {
            writtenObjects[index] = gameObject;
        } else
        {
            writtenObjects.Add(gameObject);
        }

        if (writtenObjects.Count == numberOfObjectsThatShouldHaveText)
        {
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
    public void CheckWrittenObjects()
    {
        List<WritingSpec> writingSpecs = WritingSpecifications.GetInitialRequiredWritings();

        for (int i = writtenObjects.Count - 1; i >= 0; i--)
        {
            var gameObject = writtenObjects[i];
            if (Matches(gameObject, writingSpecs, false))
            {
                writtenObjects.RemoveAt(i);
            }
        }

        if (writtenObjects.Count > 0)
        {
            for (int i = writtenObjects.Count - 1; i >= 0; i--)
            {
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
    public bool Matches(GameObject gameObject, List<WritingSpec> writingSpecs, bool checkForMistakes)
    {
        GeneralItem item = gameObject.GetComponent<GeneralItem>();
        Writable writable = gameObject.GetComponent<Writable>();
        bool correctObjectType = false;
        if (item == null || writable == null) return false;
        for (int i = writingSpecs.Count - 1; i >= 0; i--)
        {
            var writingSpec = writingSpecs[i];
            if (writingSpec.objectType == item.ObjectType)
            {
                correctObjectType = true;
                bool foundAllRequiredWritings = true;
                WritingType[] requiredWritings = writingSpec.requiredWritings;
                foreach (var requiredWriting in requiredWritings)
                {
                    if (!writable.WrittenLines.ContainsKey(requiredWriting))
                    {
                        foundAllRequiredWritings = false;
                        if (checkForMistakes) CreateTaskMistake("You have not written " + requiredWriting + " on " + item.ObjectType, 1);
                    }
                }
                if (foundAllRequiredWritings)
                {
                    int index = writingSpecs.IndexOf(writingSpec);
                    writingSpecs.RemoveAt(index);;
                    return true;
                }
            }
        }
        if (correctObjectType == false && checkForMistakes)
        {
            CreateTaskMistake("Kirjoitit ylim��r�iseen esineseen", 1);
        }
        return false;
    }

    protected override void OnTaskComplete()
    {
    }

    #region Public Methods

    public override void CompleteTask()
    {
        base.CompleteTask();
        if (Completed)
        {
            Popup("Hyvin kirjoitettu.", MsgType.Done);
        }
    }

    public override string GetHint()
    {
        return HINT;
    }
    #endregion
}