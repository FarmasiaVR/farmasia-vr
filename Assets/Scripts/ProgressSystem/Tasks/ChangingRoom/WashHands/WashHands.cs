using System;
using UnityEngine;

public class WashHands : Task {

    public enum Conditions { HandsWashed }
    private HandStateManager handStateManager;
    private HandState handState = HandState.Dirty;
    private PackageName packageName;

    public WashHands(PackageName packageName, TaskType taskType) : base(taskType, false) {
        handStateManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HandStateManager>();
        this.packageName = packageName;
        SetCheckAll(true);
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(HandsTouched, EventType.WashingHands);
    }

    // Track progress of washing hands
    private void HandsTouched(CallbackData data) {
        handStateManager.WashingHands(data);

        var liquid = (data.DataObject as HandWashingLiquid);

        // Don't track if previous tasks haven't been completed yet
        if (!Started) {
            // Punish the player if they try to wash hands before completing previous tasks
            if (packageName == PackageName.ChangingRoom) {
                // Ignoring water since it is used to wash glasses
                if (!liquid.type.Equals("Water")) CreateTaskMistake("Pue laboratoriotakki ja kengänsuojat sekä puhdista silmälasit ennen käsienpesua", 1);
            } else if (packageName == G.Instance.Progress.CurrentPackage.name) {
                CreateTaskMistake("Pue suojapäähine ja kasvomaski ennen käsienpesua", 1);
            }
            return;
        }


        if (liquid.type.Equals("HandSanitizer") && handState == HandState.Clean) {
            EnableCondition(Conditions.HandsWashed);
            CompleteTask();
        }

        // Checking for mistakes is done in HandStateManager
        if (handStateManager.GetIsMistake() == true) {
            CreateTaskMistake("Käsienpesu tulee tehdä oikeassa järjestyksessä", 1);
        }

        handState = handStateManager.GetHandState();
        Logger.Print("Liquid: " + liquid.type + " | HandState: " + handState + " | IsMistake: " + handStateManager.GetIsMistake());
    }
}
