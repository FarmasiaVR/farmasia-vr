using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class Agar : Interactable {

    private bool FirstTouch = false;
    private bool SecondTouch = false; 
    private bool isLeft;
    private System.DateTime LastTouched;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        LastTouched = System.DateTime.Now;
        
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
        string handIs = hand.HandType.ToString();

        var time = System.DateTime.Now - LastTouched;
        Logger.Print("fingerprints given for "+ time);

        if (!FirstTouch && time.Seconds > 0) {
            base.Interact(hand);
            if (handIs.Equals("LeftHand")) {
                isLeft = true;
            } else {
                isLeft = false;
            }
            FirstTouch = true;
            Logger.Print("fingerprints given");

            if (time.Seconds < 4 || time.Seconds > 6) {
                Task.CreateTaskMistake(TaskType.Fingerprints, "Kosketuksen tulee olla noin 5 sekuntia", 1);
            }

        } else if (FirstTouch && !SecondTouch) {
            base.Interact(hand);
            SecondTouch = true;
            Logger.Print("thumb print given");

            if (handIs.Equals("LeftHand")) {
                Events.FireEvent(EventType.FingerprintsGivenL);
                if (!isLeft) {
                    Task.CreateTaskMistake(TaskType.Fingerprints, "Älä sekoita sormia! Tässä on nyt vasemman käden jäljet.", 1);
                } else {
                    Logger.Print("oikein meni");
                }
            } else {
                Events.FireEvent(EventType.FingerprintsGivenR);
                if (isLeft) {
                    Task.CreateTaskMistake(TaskType.Fingerprints, "Älä sekoita sormia! Tässä on nyt oikean käden jäljet.", 1);
                } else {
                    Logger.Print("oikein meni");
                }
            }
            SecondTouch = true;
        } else {
            Logger.Print("excess touch");
        }

    }
}
