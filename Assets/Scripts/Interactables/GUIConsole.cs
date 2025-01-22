using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

// NOTE: THIS CLASS CANNOT CALL Logger METHODS, RECURSIVE INFINITE LOOP

public class GUIConsole : Movable {
    private StringBuilder stringBuilder = new StringBuilder();

    public static List<string> log;

    private float lastScrollTime;

    private int prevIndex;
    private int currentIndex;
    [SerializeField]
    private int linesToShow;

    private TextMeshPro text;

    private bool recent;

    static GUIConsole() {
        log = new List<string>();
    }

    protected override void Start() {

        #if UNITY_EDITOR
        #else
            Destroy(gameObject);
        #endif

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
    }

    public override void OnGrab(Hand hand) {
        base.OnGrab(hand);
        UpdateIndex(hand);
    }

    private void UpdateIndex(Hand hand) {

        if (VRInput.GetControl(hand.HandType, ControlType.DPadNorth)) {
            Scroll(-1);
        }

        if (VRInput.GetControl(hand.HandType, ControlType.DPadSouth)) {
            Scroll(1);
        }

        if (VRInput.GetControlDown(hand.HandType, ControlType.PadClick)) {
            ShowRecent();
        }
    }

    /// <summary>
    /// Scrolls to direction every 0.1 seconds
    /// </summary>
    /// <param name="dir"></param>
    private void Scroll(int dir) {
        if (Time.time - lastScrollTime > 0.1f) {
            currentIndex += dir;
            lastScrollTime = Time.time;
        }
        recent = false;
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
        for (int i = from - amount + 1; i <= from; i++) {
            if (InvalidIndex(i)) {
                continue;
            }
            stringBuilder.AppendLine(string.Format("{0}: {1}", i.ToString(), log[i]));
        }
        string result = stringBuilder.ToString();
        stringBuilder.Clear();

        return result;
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
            log.Add(string.Format("<color=#ffff00> Warning: </color> {0}", line));
        }
    }
    public static void LogError(string message) {
        string[] lines = message.Split('\n');
        foreach (string line in lines) {
            log.Add(string.Format("<color=#FF0000>Error: </color> {0}", line));
        }
    }
#endregion
}
