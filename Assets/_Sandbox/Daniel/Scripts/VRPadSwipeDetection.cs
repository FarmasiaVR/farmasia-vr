using UnityEngine;
using Valve.VR;

public class VRPadSwipeDetection {

    private const float TIMEOUT_SEC = 0.250f;
    // Range: [0, 2] (pad values range from [-1, 1])
    private float SWIPE_THRESHOLD = 0.75f;

    #region Delegates
    public delegate void SwipeCallback();
    #endregion

    #region Fields
    public SwipeCallback OnSwipeLeft { get; set; }
    public SwipeCallback OnSwipeRight { get; set; }
    public SwipeCallback OnSwipeUp { get; set; }
    public SwipeCallback OnSwipeDown { get; set; }
    private SteamVR_Input_Sources handType;
    private Pipeline timeout;
    // isSwiping is true if a pad down event
    // has fired and the timeout has not yet triggered
    private bool isSwiping;
    private float startX, startY;
    private float deltaX, deltaY;
    #endregion

    public VRPadSwipeDetection(SteamVR_Input_Sources handType) {
        this.handType = handType;
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
                    timeout = new Pipeline().Delay(TIMEOUT_SEC).Func(Reset);
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
                if (deltaX > SWIPE_THRESHOLD) {
                    OnSwipeRight?.Invoke();
                } else if (deltaX < -SWIPE_THRESHOLD) {
                    OnSwipeLeft?.Invoke();
                }

                if (deltaY > SWIPE_THRESHOLD) {
                    OnSwipeUp?.Invoke();
                } else if (deltaY < -SWIPE_THRESHOLD) {
                    OnSwipeDown?.Invoke();
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