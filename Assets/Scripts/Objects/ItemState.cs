public static class ItemState {

    public enum Status {
        Clean = 0,
        Broken = 1
    }

    public static void SetFlags(ref int flags, bool value, params Status[] statuses) {
        foreach (Status status in statuses) {
            SetFlag(ref flags, status, value);
        }
    }

    private static void SetFlag(ref int flags, Status status, bool value) {
        if (value) flags = flags | StatusToInt(status);
        else flags = flags & ~StatusToInt(status);
    }

    public static bool GetFlag(int flags, Status status) {
        return (flags & StatusToInt(status)) != 0;
    }

    private static int StatusToInt(Status status) {
        return 1 << (int)status;
    }
}