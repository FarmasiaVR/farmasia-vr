using UnityEngine;
using Valve.VR;

/// VRPadClick handles pad touches and splits them into middle, left, right, up
/// and down clicks. Left, right, up and down are only triggered if the touch is
/// outside of the middle area (radius).
public class VRPadClick {

    #region Delegates
    public delegate void ClickCallback();
    #endregion

    #region Fields
    public ClickCallback OnClickMiddle { get; set; }
    public ClickCallback OnClickLeft { get; set; }
    public ClickCallback OnClickRight { get; set; }
    public ClickCallback OnClickUp { get; set; }
    public ClickCallback OnClickDown { get; set; }
    public bool IsDown { get; private set; }

    private SteamVR_Input_Sources handType;
    private float middleRadiusSqrd;
    #endregion

    /// <param name="isLeftHand">True for left hand pad, false for right hand pad</param>
    /// <param name="middleRadius">The radius for the middle area. Range: [0,1]</param>
    public VRPadClick(bool isLeftHand, float middleRadius) {
        this.middleRadiusSqrd = middleRadius * middleRadius;
        this.handType = isLeftHand ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;
    }

    public void Update(float deltaTime) {
        if (VRInput.GetControlDown(handType, ControlType.PadTouch)) {
            Vector2 touch = VRInput.PadTouchValue(handType);

            float distSqrd = touch.x * touch.x + touch.y * touch.y;
            if (distSqrd < middleRadiusSqrd) {
                TouchMiddle();
                return;
            }

            if (touch.x < 0)        TouchLeft();
            else if (touch.x > 0)   TouchRight();
            if (touch.y < 0)        TouchDown();
            else if (touch.y > 0)   TouchUp();

            IsDown = true;
        }

        if (VRInput.GetControlUp(handType, ControlType.PadTouch)) {
            IsDown = false;
        }
    }

    private void TouchMiddle() {
        OnClickMiddle?.Invoke();
    }

    private void TouchLeft() {
        OnClickLeft?.Invoke();
    }

    private void TouchRight() {
        OnClickRight?.Invoke();
    }
    
    private void TouchUp() {
        OnClickUp?.Invoke();
    }

    private void TouchDown() {
        OnClickDown?.Invoke();
    }
}