public struct ControlState {

    public ControlState(int getDown, bool down, int getUp) {
        this.GetDown = getDown;
        this.Down = down;
        this.GetUp = getUp;
    }

    public int GetDown;
    public bool Down;
    public int GetUp;
}