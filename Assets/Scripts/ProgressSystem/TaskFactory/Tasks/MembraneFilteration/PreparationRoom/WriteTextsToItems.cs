using UnityEngine;
using System;
public class WriteTextsToItems : TaskBase
{
    /*private class Item
    {
        public int instanceId;
        public WritingType liquid;
        public WritingType name;
        public WritingType date;
        public WritingType time;
        private Item(int instanceId)
        {
            this.instanceId = instanceId;
        }
    }*/
    // Instead, use WritingSpecifications.GetInitialRequiredWritings()

    #region Constants
    private const string DESCRIPTION = "Kirjoita tarvittavat tiedot pulloihin ja maljoihin";
    private const string HINT = "Kosketa kynällä esinettä, johon haluat kirjoittaa, valitse kirjoitettavat tekstit (max 4) klikkaamalla niitä. Voit perua kirjoituksen painamalla tekstiä uudestaan ennen kuin painat vihreää nappia";
    #endregion

    #region Fields
    /// <summary>
    /// Conditions must be met to render task complete
    /// </summary>
    public enum Conditions { SoycaseinePlatesHaveText, SabouradDextrosiPlateHasText, BottlesHaveText }
    private int bottles = 0;
    private int soycaseinePlates = 0;
    private int sabouradPlates = 0;
    private int numberOfObjectsThatShouldHaveText = 1;
    #endregion

    public WriteTextsToItems() : base(TaskType.WriteTextsToItems, true, false)
    {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
        points = 2;
    }

    #region Event Subscriptions
    public override void Subscribe()
    {
        base.SubscribeEvent(TrackWrittenObjects, EventType.WriteToObject);
    }

    private void TrackWrittenObjects(CallbackData data)
    {
        GameObject gameObject = (GameObject)data.DataObject;
        ObjectType objectType = gameObject.GetComponent<GeneralItem>().ObjectType;
        Logger.Print("OBJECTID: " + gameObject.GetInstanceID());
        Logger.Print("Progress system object objectType: " + objectType);
        Writable textComponent = gameObject.GetComponent<Writable>();
        Logger.Print("Progress sytemiä varten tekstin löytyminen: " + textComponent.Text);
        foreach (var option in textComponent.WrittenLines)
            Logger.Print(option.Key + " = " + option.Value);


        if (objectType == ObjectType.Bottle)
        {
            bottles++;
            if (bottles == 1)
            {
                EnableCondition(Conditions.BottlesHaveText);
                EnableCondition(Conditions.SoycaseinePlatesHaveText);
                EnableCondition(Conditions.SabouradDextrosiPlateHasText);
            }
        }
        if (objectType == ObjectType.SoycaseinePlate)
        {
            soycaseinePlates++;
        }
        if (objectType == ObjectType.SabouradDextrosiPlate)
        {
            sabouradPlates++;
        }

        if (bottles == numberOfObjectsThatShouldHaveText)
        {
            CompleteTask();
        }

    }
    #endregion
    protected override void OnTaskComplete()
    {
    }

    #region Public Methods

    public override void CompleteTask()
    {
        base.CompleteTask();
        if (IsCompleted())
        {
            Popup("Hyvin kirjoitettu.", MsgType.Done);
        }
    }

    public override string GetDescription()
    {
        return DESCRIPTION;
    }

    public override string GetHint()
    {
        return HINT;
    }
    #endregion
}