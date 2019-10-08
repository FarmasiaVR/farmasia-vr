using UnityEngine;

public class VRPadSwipeTest : MonoBehaviour {

    private VRPadSwipeDetection swipe = new VRPadSwipeDetection(false, 0.75f, 0.25f);

    private void Awake() {
        swipe.OnSwipeRight = (dx) => Logger.Print("Swipe right, delta: " + dx);
        swipe.OnSwipeLeft = (dx) => Logger.Print("Swipe left, delta: " + dx);
        swipe.OnSwipeDown = (dy) => Logger.Print("Swipe down, delta: " + dy);
        swipe.OnSwipeUp = (dy) => Logger.Print("Swipe up, delta: " + dy);
    }

    private void Update() {
        swipe.Update(Time.deltaTime);
    }
}
