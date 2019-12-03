using UnityEngine;

public class VRPadTest : MonoBehaviour {

    #region Fields
    [SerializeField]
    private bool testSwipe = true;
    [SerializeField]
    private bool testClick = true;

    private VRPadSwipe swipe;
    private VRPadClick click;
    #endregion

    private void Awake() {
        swipe = new VRPadSwipe(G.Instance.Pipeline, false, 0.75f, 0.25f);
        swipe.OnSwipeRight = (dx) => Logger.Print("Swipe right, delta: " + dx);
        swipe.OnSwipeLeft = (dx) => Logger.Print("Swipe left, delta: " + dx);
        swipe.OnSwipeDown = (dy) => Logger.Print("Swipe down, delta: " + dy);
        swipe.OnSwipeUp = (dy) => Logger.Print("Swipe up, delta: " + dy);

        click = new VRPadClick(false, 0.4f);
        click.OnClickMiddle = (x, y) => Logger.Print("Click middle: (" + x + "," + y + ")");
        click.OnClickLeft = (x, y) => Logger.Print("Click left: (" + x + ", " + y + ")");
        click.OnClickRight = (x, y) => Logger.Print("Click right: (" + x + "," + y + ")");
        click.OnClickDown = (x, y) => Logger.Print("Click down: (" + x + "," + y + ")");
        click.OnClickUp = (x, y) => Logger.Print("Click up: (" + x + "," + y + ")");
    }

    private void Update() {
        if (testSwipe) swipe.Update(Time.deltaTime);
        if (testClick) click.Update(Time.deltaTime);
    }
}
