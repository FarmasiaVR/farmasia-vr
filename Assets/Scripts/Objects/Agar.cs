using System;

public class Agar : Interactable {

    private DateTime lastTouched;
    private int leftHandTouches;
    private int rightHandTouches;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
        leftHandTouches = 0;
        rightHandTouches = 0;
    }

    public override void Interact(Hand hand) {
        lastTouched = DateTime.Now;
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
        string handIs = hand.HandType.ToString();
        TimeSpan time = DateTime.Now - lastTouched;
        Logger.Print("Fingerprints given for " + time + " seconds");
        base.Interact(hand);
        if (handIs.Equals("LeftHand")) {
            leftHandTouches++;
        } else {
            rightHandTouches++;
        }
        if (time.Seconds < 4.0f || time.Seconds > 6.0f) {
            Task.CreateTaskMistake(TaskType.Fingerprints, "Kosketuksen tulee olla noin 5 sekuntia", 1);
        }
        if (leftHandTouches >= 2) {
            Events.FireEvent(EventType.FingerprintsGivenL);
        } else if (rightHandTouches >= 2) {
            Events.FireEvent(EventType.FingerprintsGivenR);
        }
    }
}
