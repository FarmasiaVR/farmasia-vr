using UnityEngine;
using Valve.VR;

public class VRPadSwipeDetection {

    #region Delegates
    public delegate void SwipeCallback(float delta);
    #endregion

    #region Fields
    public SwipeCallback OnSwipeLeft { get; set; }
    public SwipeCallback OnSwipeRight { get; set; }
    public SwipeCallback OnSwipeUp { get; set; }
    public SwipeCallback OnSwipeDown { get; set; }
    private SteamVR_Input_Sources handType;
    private Pipeline timeout;
    private float timeoutSec;
    // Range: [0, 2] (pad values range from [-1, 1])
    private float swipeThreshold;
    // isSwiping is true if a pad down event
    // has fired and the timeout has not yet triggered
    private bool isSwiping;
    private float startX, startY;
    private float deltaX, deltaY;
    #endregion

    /// <param name="handType">True for left hand pad, false for right hand pad</param>
    /// <param name="swipeThreshold">How long the swipe needs to be to trigger the callback, range: [0, 2]</param>
    /// <param name="timeoutSec">How long the swipe is allowed to last before becoming invalid</param>
    public VRPadSwipeDetection(bool leftHand, float swipeThreshold, float timeoutSec) {
        this.handType = leftHand ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;
        this.swipeThreshold = Mathf.Min(Mathf.Max(swipeThreshold, 0), 2);
        this.timeoutSec = timeoutSec;
        Reset();
    }

    public void Update(float deltaTime) {
        timeout.Update(deltaTime);

        if (VRInput.GetControl(handType, ControlType.PadTouch)) {
            Vector2 pos = VRInput.PadTouchValue(handType);
            Vector2 delta = VRInput.PadTouchDelta(handType);
            if (isSwiping) {
                // Initial delta value is calculated from the initial touch position and the current position
                // PadTouchDelta returns the position since the previous position was (0, 0) when not touched
                if (deltaX == 0 && deltaY == 0) {
                    deltaX = pos.x - startX;
                    deltaY = pos.y - startY;
                    timeout = new Pipeline().Delay(timeoutSec).Func(Reset);
                } else {
                    deltaX += delta.x;
                    deltaY += delta.y;
                }
            }
        }

        if (VRInput.GetControlDown(handType, ControlType.PadTouch)) {
            Vector2 pos = VRInput.PadTouchValue(handType);
            startX = pos.x;
            startY = pos.y;
            isSwiping = true;
        }
        
        if (VRInput.GetControlUp(handType, ControlType.PadTouch)) {
            if (isSwiping) {
                if (deltaX > swipeThreshold) {
                    OnSwipeRight?.Invoke(deltaX);
                } else if (deltaX < -swipeThreshold) {
                    OnSwipeLeft?.Invoke(deltaX);
                }

                if (deltaY > swipeThreshold) {
                    OnSwipeUp?.Invoke(deltaX);
                } else if (deltaY < -swipeThreshold) {
                    OnSwipeDown?.Invoke(deltaX);
                }

                Reset();
            }
        }
    }

    private void Reset() {
        timeout = new Pipeline();
        isSwiping = false;
        deltaX = 0;
        deltaY = 0;
        startX = 0;
        startY = 0;
    }
}