using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

// NOTE: THIS CLASS CANNOT CALL Logger METHODS, RECURSIVE INFINITE LOOP

public class GUIConsole : Movable {

    #region Fields
    public static List<string> log;
    private static float scrollTreshold = 0.01f;

    private bool touch;

    private int prevIndex;
    private int currentIndex;
    [SerializeField]
    private int linesToShow;

    private TextMeshPro text;

    private bool recent;
    #endregion

    static GUIConsole() {
        log = new List<string>();
    }

    protected override void Start() {
        base.Start();

        text = transform.Find("Text").GetComponent<TextMeshPro>();

        recent = true;
    }

    private void Update() {
        if (recent) {
            ShowRecent();
        } else {
            ShowFromIndex();
        }
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);

        recent = true;

        StartCoroutine(DisableAccidentalScroll());
    }
    private IEnumerator DisableAccidentalScroll() {
        float time = 0.5f;

        while (time > 0) {
            touch = false;
            time -= Time.deltaTime;
            yield return null;
        }
    }

    public override void Interacting(Hand hand) {
        base.Interacting(hand);
        UpdateIndex(hand);
    }

    private void UpdateIndex(Hand hand) {

        if (VRInput.GetControlDown(hand.HandType, ControlType.PadTouch)) {
            touch = true;
        }

        if (!touch) {
            return;
        }

        if (VRInput.GetControl(hand.HandType, ControlType.PadTouch)) {
            int swipe = SwipeDirection(VRInput.PadTouchValue(hand.HandType), VRInput.PadTouchDelta(hand.HandType));
            if (swipe != 0) {
                currentIndex += swipe;
                recent = false;
            }
        }
    }

    private int SwipeDirection(Vector2 current, Vector2 delta) {

        if (delta.y > scrollTreshold) {
            return 1;
        } else if (delta.y < -scrollTreshold) {
            return -1;
        }
        return 0;
    }

    private void ShowRecent() {
        if (log.Count == 0 || currentIndex == log.Count - 1) {
            return;
        }

        currentIndex = log.Count - 1;
        prevIndex = currentIndex;
        text.text = GetMessages(currentIndex, linesToShow);
    }

    private void ShowFromIndex() {
        if (log.Count == 0 || prevIndex == currentIndex) {
            return;
        }

        prevIndex = currentIndex;

        text.text = GetMessages(currentIndex, linesToShow);
    }

    private string GetMessages(int from, int amount) {
        StringBuilder b = new StringBuilder();

        for (int i = from - amount + 1; i <= from; i++) {
            if (InvalidIndex(i)) {
                continue;
            }
            b.AppendLine(i + ": " + log[i]);
        }

        return b.ToString();
    }
    private bool InvalidIndex(int i) {
        return i < 0 || i >= log.Count;
    }


    #region Static methods
    public static void Log(string message) {
        string[] lines = message.Split('\n');
        foreach (string line in lines) {
            log.Add(line);
        }
    }
    public static void LogWarning(string message) {
        string[] lines = message.Split('\n');
        foreach (string line in lines) {
            log.Add("<color=#ffff00> Warning: </color>" + line);
        }
    }
    public static void LogError(string message) {
        string[] lines = message.Split('\n');
        foreach (string line in lines) {
            log.Add("<color=#FF0000>Error: </color>" + line);
        }
    }
    #endregion
}
