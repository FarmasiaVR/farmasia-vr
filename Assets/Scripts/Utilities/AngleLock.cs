public static class AngleLock {
    public static float TrimAngleDeg(float current) {
        float angle = current;
        while (angle < 0)
            angle += 360;
        while (angle >= 360)
            angle -= 360;
        return angle;
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
