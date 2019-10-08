using UnityEngine;
using Valve.VR;

public class VRPadSwipeTest : MonoBehaviour
{

    private VRPadSwipeDetection swipe = new VRPadSwipeDetection(SteamVR_Input_Sources.RightHand);

    private void Awake() {
        swipe.OnSwipeRight = () => { Logger.Print("Yay callback"); };
    }

    private void Update() {
        swipe.Update(Time.deltaTime);
    }
}
