

public class Callback {

    public delegate void VoidCallback();

    private VoidCallback callbacks;

    public void AddCallback(VoidCallback c) {
        callbacks += c;
    }
    public void RemoveCallback(VoidCallback c) {
        callbacks -= c;
    }
    public void Reset() {
        callbacks = null;
    }
    public void Invoke() {
        callbacks?.Invoke();
    }
}