using System;

public static class AngleLock {

    public static float TrimAngleDeg(float current) {
        float angle = current;
        while (angle < 0)
            angle += 360;
        while (angle >= 360)
            angle -= 360;
        return angle;
    }

    public static float TrimAngleDeltaDeg(float delta) {
        float angle = delta;
        while (angle < -180)
            angle += 360;
        while (angle >= 180)
            angle -= 360;
        return angle;
    }

    /// <summary>
    /// Attempts to change currentAngle by angleDelta and clamps the resulting angle
    /// between the left and right angles.
    /// 
    /// Note: Unity angles increase clockwise, therefore the clamped area is defined clockwise from left to right.
    /// </summary>
    /// <param name="currentAngle">The angle to clamp</param>
    /// <param name="leftClampAngle">The left clamp limit</param>
    /// <param name="rightClampAngle">The right clamp limit</param>
    /// <param name="angleDelta">Current angle speed</param>
    /// <returns>The clamped angle</returns>
    public static float ClampAngleDeg(float currentAngle, float leftClampAngle, float rightClampAngle, float angleDelta = 0) {
        currentAngle = TrimAngleDeg(currentAngle);
        leftClampAngle = TrimAngleDeg(leftClampAngle);
        rightClampAngle = TrimAngleDeg(rightClampAngle);
        angleDelta = TrimAngleDeltaDeg(angleDelta);

        float leftRelativeToCurrent = TrimAngleDeg(leftClampAngle - currentAngle);
        if (leftRelativeToCurrent == 0) leftRelativeToCurrent = 360;
        float rightRelativeToCurrent = TrimAngleDeg(rightClampAngle - currentAngle);

        // If currentAngle is not inside clamped region, (currentAngle is out of bounds)
        // clamp to nearest clamp limit.
        if (leftRelativeToCurrent < rightRelativeToCurrent) {
            if (leftRelativeToCurrent < 360 - rightRelativeToCurrent) return leftClampAngle;
            else return rightClampAngle;
        }

        // Needed for angle comparison below.
        // Invariant: leftRelativeToCurrent < current < rightRelativeToCurrent
        leftRelativeToCurrent -= 360;

        if (angleDelta > rightRelativeToCurrent) {
            return rightClampAngle;
        } else if (angleDelta < leftRelativeToCurrent) {
            return leftClampAngle;
        } else {
            return TrimAngleDeg(angleDelta + currentAngle);
        }
    }

    public static float ClampAngleDegSteps(float current, float left, float right, float angleDelta, float maxDeltaAngle) {
        angleDelta = TrimAngleDeltaDeg(angleDelta);
        maxDeltaAngle = Math.Abs(maxDeltaAngle);

        float deltaRemaining = angleDelta;
        float curAngle = current;
        bool positiveDelta = angleDelta > 0;

        do {
            float step = positiveDelta ? Math.Min(deltaRemaining, maxDeltaAngle) : Math.Max(deltaRemaining, -maxDeltaAngle);
            curAngle = ClampAngleDeg(curAngle + step, left, right);
            deltaRemaining -= step;
        } while ((positiveDelta && deltaRemaining > 0) || (!positiveDelta && deltaRemaining < 0));
        
        return curAngle;
    }

    public static float ClampAngleDeg(float current, float left, float right) {
        float offset = (left + right) / 2 + 180;
        float fakeLeft = TrimAngleDeg(left - offset);
        float fakeRight = TrimAngleDeg(right - offset);
        float angle = TrimAngleDeg(current - offset);

        if (angle < fakeLeft) {
            return TrimAngleDeg(fakeLeft + offset);
        } else if (angle > fakeRight) {
            return TrimAngleDeg(fakeRight + offset);
        } else {
            return TrimAngleDeg(angle + offset);
        }
    }
}
