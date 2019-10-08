using Valve.VR;

public class VRPadSwipeDetection {

    private const float TIMEOUT_MS = 250;
    // Percentage of pad radius
    // Pad values range from [-1, 1]
    private float SWIPE_THRESHOLD = 0.2f;

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
    // VRInput.[Left|Right]PadDelta can maybe be used,
    // depending on when and how the value is changed
    private float startX, startY;
    #endregion

    public VRPadSwipeDetection(SteamVR_Input_Sources handType) {
        this.handType = handType;
        timeout = new Pipeline();
    }

    private void Update(float deltaTime) {
        timeout.Update(deltaTime);

        if (VRInput.GetControlDown(handType, ControlType.PadTouch)) {
            isSwiping = true;
            startX = VRInput.PadTouchValue(handType).x;
            startY = VRInput.PadTouchValue(handType).y;
            timeout = new Pipeline().Delay(TIMEOUT_MS).Func(Reset);
        }

        if (isSwiping && VRInput.GetControlUp(handType, ControlType.PadTouch)) {
            float deltaX = VRInput.PadTouchValue(handType).x - startX;
            float deltaY = VRInput.PadTouchValue(handType).y - startY;
            if (deltaX > deltaY) {
                if (deltaX > SWIPE_THRESHOLD) {
                    Logger.Print("Swipe right");
                    OnSwipeRight?.Invoke();
                } else if (deltaX < SWIPE_THRESHOLD) {
                    Logger.Print("Swipe left");
                    OnSwipeLeft?.Invoke();
                }

                Reset();
            } else if (deltaY > deltaX) {
                if (deltaY > SWIPE_THRESHOLD) {
                    Logger.Print("Swipe up");
                    OnSwipeUp?.Invoke();
                } else if (deltaY < SWIPE_THRESHOLD) {
                    Logger.Print("Swipe down");
                    OnSwipeDown?.Invoke();
                }

                Reset();
            }
        }
    }

    private void Reset() {
        timeout = new Pipeline();
        isSwiping = false;
        startX = 0;
        startY = 0;
    }
}