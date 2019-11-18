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

    public static float ClampAngleDeg(float current, float left, float right, float angleDelta = 0, float maxDeltaAngle = 10) {
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
