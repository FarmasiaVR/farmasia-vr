using UnityEngine;
using System;
public class SelectToolsMembrane : Task {

    #region Constants
    public override string Description {
        get => "Valitse sopivat ty�v�lineet.";
    }
    private const string HINT = "Huoneessa on l��kkeen valmistukseen tarvittavia ty�v�lineit�. Valitse oikea m��r� ruiskuja, neuloja ja luerlockeja.";
    #endregion

    #region Fields
    public enum Conditions {
        SmallBottlePickedUp, PeptoniWaterPickedUp, SoycaseineBottlePickedUp, TioglycolateBottlePickedUp, TweezersPickedUp, ScalpelPickedUp, PipettePickedUp
    }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor for SelectTools task. 
    /// Is removed when finished and doesn't require previous task completion.
    /// </summary>
    public SelectToolsMembrane() : base(TaskType.SelectToolsMembrane, true, false) {
        SetCheckAll(false);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }
    #endregion

    #region Event Subscriptions
    public override void Subscribe() {
        base.SubscribeEvent(PickupObject, EventType.PickupObject);
    }

    /// <summary>
    /// Once fired by an event, checks if the item is correct and sets corresponding condition to be true.
    /// </summary>
    /// <param name="data">Refers to the GameObject that was picked up.</param>
    private void PickupObject(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        /*switch (type)
        {
            case ObjectType.Syringe:
                EnableCondition(Conditions.SyringePickedUp);
                break;
            case ObjectType.Needle:
                EnableCondition(Conditions.NeedlePickedUp);
                break;
            case ObjectType.Luerlock:
                EnableCondition(Conditions.LuerlockPickedUp);
                break;
            case ObjectType.SyringeCapBag:
                EnableCondition(Conditions.SyringeCapBagPickedUp);
                break;
            case ObjectType.Pen:
                EnableCondition(Conditions.Pen);
                break;
        }*/
        CompleteTask();
    }
    #endregion

    protected override void OnTaskComplete() {
        Popup("Ty�v�line valittu.", MsgType.Done);
    }

    #region Public Methods

    public override void FinishTask() {
        base.FinishTask();

    }

    public override string Hint {
        get => HINT;
    }
    #endregion
}