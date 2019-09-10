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
        float angle = TrimAngleDeg(current);
        float leftTrimmed = TrimAngleDeg(left);
        float rightTrimmed = TrimAngleDeg(right);
        float divider = TrimAngleDeg((rightTrimmed + leftTrimmed) / 2f + 180);
        bool isOnLeft = TrimAngleDeg(angle - divider) < 180;

        if (angle > leftTrimmed && isOnLeft)
            return leftTrimmed;
        if (angle < rightTrimmed && !isOnLeft)
            return rightTrimmed;
        return angle;
    }
}
