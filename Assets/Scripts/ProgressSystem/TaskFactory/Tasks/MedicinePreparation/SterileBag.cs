using UnityEngine;
public class SterileBag : TaskBase {
    #region Fields
    private string[] conditions = { "TwoSyringesPut" };
    private int itemCount;
    #endregion

    #region Constructor
    ///  <summary>
    ///  Constructor for SterileBag task.
    ///  Is removed when finished and doesn't require previous task completion.
    ///  </summary>
    public SterileBag() : base(TaskType.SterileBag, true, false) {
        itemCount = 0;
        Subscribe();
        AddConditions(conditions);
    }
    #endregion

    #region Event Subscriptions
    /// <summary>
    /// Subscribes to required Events.
    /// </summary>
    public override void Subscribe() {
        base.SubscribeEvent(PutToBag, EventType.SterileBag);
    }
    /// <summary>
    /// Once fired by an event, checks how many syringe objects are put to bag object.
    /// Sets corresponding condition to be true.
    /// </summary>
    /// <param name="data">"Refers to the data returned by the trigger."</param>
    private void PutToBag(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        ObjectType type = item.ObjectType;
        if (type == ObjectType.Syringe) {
            ///code missing, check that the syringe object is placed in a sterile bag object
            itemCount++;
            if (itemCount == 2) {
                EnableCondition("TwoSyringesPut");
            }
        }
        CheckClearConditions(true);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Once all conditions are true, this method is called.
    /// </summary>
    public override void FinishTask() {
        UISystem.Instance.CreatePopup("Syringes put in sterile bag", MessageType.Done);
        base.FinishTask();
    }
    
    /// <summary>
    /// Used for getting the task's description.
    /// </summary>
    /// <returns>"Returns a String presentation of the description."</returns>
    public override string GetDescription() {
        return "Viimeistele ruiskujen kanssa työskentely.";
    }

    /// <summary>
    /// Used for getting the hint for this task.
    /// </summary>
    /// <returns>"Returns a String presentation of the hint."</returns>
    public override string GetHint() {
        return "Laita molemmat käyttämäsi ruiskut steriiliin pussiin.";
    }
    #endregion
}