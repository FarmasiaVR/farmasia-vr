public static class BitField {

    public static void SetFlags(ref int flags, bool value, params object[] statuses) {
        foreach (object status in statuses) {
            SetFlag(ref flags, (int)status, value);
        }
    }

    private static void SetFlag(ref int flags, int status, bool value) {
        if (value) {
            flags = flags | StatusToInt(status);
        } else {
            flags = flags & ~StatusToInt(status);
        }
        int asd = 0;
        SetFlags(ref asd, true, Control.Grip, Control.Menu);
    }

    public static bool GetFlags(int flags, params object[] statuses) {
        bool all = true;
        foreach (object status in statuses) {
            bool t = (flags & StatusToInt(status)) != 0;

            if (!t) {
                all = false;
            }
        }
        return all;
    }

    private static int StatusToInt(object status) {
        return 1 << (int)status;
    }
}
