using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

// NOTE: THIS CLASS CANNOT CALL Logger METHODS, RECURSIVE INFINITE LOOP

public class GUIConsole : Interactable {

    #region Fields
    public static List<string> log;

    private int currentIndex;
    [SerializeField]
    private int linesToShow;

    private TextMeshPro text;

    private bool recent;
    #endregion

    static GUIConsole() {
        log = new List<string>();
    }

    protected override void Awake() {
        base.Awake();

        Type.On(InteractableType.Interactable);
    }

    protected override void Start() {
        base.Start();

        text = transform.Find("Text").GetComponent<TextMeshPro>();

        ShowRecent();
    }

    private void Update() {
        if (recent) {
            ShowRecent();
        } else {
            ShowFromIndex();
        }
    }


    public override void Interacting(Hand hand) {
        base.Interacting(hand);

        transform.position = hand.Offset.position;
        transform.rotation = hand.Offset.rotation;

        UpdateIndex();
    }

    private void UpdateIndex() {

    }

    private void ShowRecent() {
        recent = true;

        if (log.Count == 0 || currentIndex == log.Count - 1) {
            return;
        }

        currentIndex = log.Count - 1;
        text.text = GetMessages(currentIndex, linesToShow);
    }

    private void ShowFromIndex() {

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
            log.Add("Warning: " + line);
        }
    }
    public static void LogError(string message) {
        string[] lines = message.Split('\n');
        foreach (string line in lines) {
            log.Add("Error: " + line);
        }
    }
    #endregion
}
