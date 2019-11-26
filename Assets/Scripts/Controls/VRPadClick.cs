using UnityEngine;
using Valve.VR;

/// VRPadClick handles pad touches and splits them into middle, left, right, up
/// and down clicks. Left, right, up and down are only triggered if the touch is
/// outside of the middle area (radius).
public class VRPadClick {

    #region Delegates
    public delegate void ClickCallback(float x, float y);
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
        UpdateNew(deltaTime);
    }

    private void UpdateOld(float deltaTime) {
        if (VRInput.GetControlDown(handType, ControlType.PadClick)) {
            Vector2 touch = VRInput.PadTouchValue(handType);
            float x = touch.x;
            float y = touch.y;

            float distSqrd = x * x + y * y;
            if (distSqrd < middleRadiusSqrd) {
                TouchMiddle(x, y);
                return;
            }

            if (touch.x < 0)        TouchLeft(x, y);
            else if (touch.x > 0)   TouchRight(x, y);
            if (touch.y < 0)        TouchDown(x, y);
            else if (touch.y > 0)   TouchUp(x, y);

            IsDown = true;
        }

        if (VRInput.GetControlUp(handType, ControlType.PadClick)) {
            IsDown = false;
        }
    }

    private void UpdateNew(float deltaTime) {
        Vector2 touch = VRInput.PadTouchValue(handType);
        float x = touch.x;
        float y = touch.y;

        if (VRInput.GetControlDown(handType, ControlType.DPadCenter)) {
            OnClickMiddle?.Invoke(x, y);
            return;
        }

        if (VRInput.GetControlDown(handType, ControlType.DPadNorth)) {
            OnClickUp?.Invoke(x, y);
        } else if (VRInput.GetControlDown(handType, ControlType.DPadSouth)) {
            OnClickDown?.Invoke(x, y);
        }
        if (VRInput.GetControlDown(handType, ControlType.DPadWest)) {
            OnClickLeft?.Invoke(x, y);
        } else if (VRInput.GetControlDown(handType, ControlType.DPadEast)) {
            OnClickRight?.Invoke(x, y);
        }
    }

    private void TouchMiddle(float x, float y) {
        OnClickMiddle?.Invoke(x, y);
    }

    private void TouchLeft(float x, float y) {
        OnClickLeft?.Invoke(x, y);
    }

    private void TouchRight(float x, float y) {
        OnClickRight?.Invoke(x, y);
    }
    
    private void TouchUp(float x, float y) {
        OnClickUp?.Invoke(x, y);
    }

    private void TouchDown(float x, float y) {
        OnClickDown?.Invoke(x, y);
    }
}