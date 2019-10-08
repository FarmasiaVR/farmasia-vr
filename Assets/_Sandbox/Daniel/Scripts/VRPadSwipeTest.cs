using UnityEngine;

public class VRPadSwipeTest : MonoBehaviour {

    private VRPadSwipeDetection swipe = new VRPadSwipeDetection(false, 0.75f, 0.25f);

    private void Awake() {
        swipe.OnSwipeRight = () => { Logger.Print("Yay callback"); };
    }

    private void Update() {
        swipe.Update(Time.deltaTime);
    }
}
