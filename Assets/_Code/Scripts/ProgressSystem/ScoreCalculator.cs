using System;
using System.Linq;
using System.Collections.Generic;
using FarmasiaVR.Legacy;

[Serializable]
public class ScoreCalculator {

    Dictionary<TaskType, int> points;
    Dictionary<TaskType, int> maxPoints;
    private int maxScore = 0;

    public Dictionary<string, int> Mistakes {
        get; private set;
    }
    public Dictionary<TaskType, HashSet<string>> TaskMistakes {
        get; private set;
    }

    /// <summary>
    /// Initializes ScoreCalculator for point calculation.
    /// </summary>
    public ScoreCalculator(List<Task> allTasks) {
        points = new Dictionary<TaskType, int>();
        TaskMistakes = new Dictionary<TaskType, HashSet<string>>();
        Mistakes = new Dictionary<string, int>();
        AddTasks(allTasks);
    }

    public void CreateMistake(string mistake, int minusPoints = 1) {
        if (Mistakes.ContainsKey(mistake)) {
            Mistakes[mistake] += minusPoints;
        } else {
            Mistakes.Add(mistake, minusPoints);
        }
    }

    public void CreateTaskMistake(TaskType type, string mistake) {
        if (!TaskMistakes.ContainsKey(type)) {
            TaskMistakes.Add(type, new HashSet<string>());
        }
        TaskMistakes[type].Add(mistake);
    }

    private void AddTasks(List<Task> tasks) {
        maxPoints = new Dictionary<TaskType, int>();
        foreach (Task task in tasks) {
            points.Add(task.TaskType, task.Points);
            maxScore += task.Points;
            maxPoints.Add(task.TaskType, task.Points);
        }
    }

    /// <summary>
    /// Subtracts a point from given task.
    /// </summary>
    /// <param name="task">Refers to a task to subtract given points.</param>
    /// <param name="subtractScore">Gives the amount of points to be subtracted.</param>
    public void SubtractPoints(TaskType task, int subtractScore) {
        if (subtractScore < 0) {
            throw new Exception("Cannot subtract negative score!");
        }
        if (!points.ContainsKey(task)) {
            return;
        }
        points[task] -= subtractScore;
    }

    public void SetScoreToZero(TaskType task) {
        if (!points.ContainsKey(task)) {
            return;
        }
        points[task] = 0;
    }

    private enum Colour {
        Yellow,
        Black,
        Green,
        Red,
        White,
        Blue
    }

    private static readonly Dictionary<Colour, string> ColourCodes = new Dictionary<Colour, string>() {
        { Colour.Yellow, "<color=#ebe134>" },
        { Colour.Black, "<color=#000000>" },
        { Colour.Green, "<color=#2dcc27>" },
        { Colour.Red, "<color=#cc3727>" },
        { Colour.White, "<color=#ffffff>" },
        { Colour.Blue, "<color=#27c7cc>" }
    };

    private string Text(string message, Colour colour) => ColourCodes[colour] + message + "</color>";

    private string PrintPoints(int gottenPoints, int maxPoints) {
        string text = gottenPoints.ToString();
        text = Text(text,
            gottenPoints < maxPoints ? Colour.Red : Colour.Green
        );
        return text + "/" + Text(maxPoints.ToString(), Colour.Green);
    }

    public Tuple<int, string> GetScoreString() {
        string totalPoints = Translator.Translate("DressingRoom", "TotalPoints");
        string summary = Translator.Translate("DressingRoom", "Congrats") + " " + Text(Player.Info.Name, Colour.Blue)  + ", " + Translator.Translate("DressingRoom", "GameOver") + "!\n\n";
        string scoreCountPerTask = "";
        string generalMistakes = "\n\n" + Translator.Translate("DressingRoom", "CommonMistakes") + ":\n";
        int score = 0;

        Logger.Print(points.Keys.Count);
        foreach (TaskType type in points.Keys) {
            if (maxPoints[type] == 0) continue;

            scoreCountPerTask += "\n " + PrintPoints(points[type], maxPoints[type]) + " : " + TaskConfig.For(type).Name;
            if (TaskMistakes.ContainsKey(type)) {
                foreach (string mistake in TaskMistakes[type]) {
                    scoreCountPerTask += "\n   " + Text("- " + mistake, Colour.Red);
                }
            }
            score += points[type];
        }

        generalMistakes = Mistakes.Count == 0 ? "" : generalMistakes;

        foreach (var pair in Mistakes) {
            generalMistakes += "\n" + Text("-" + pair.Value.ToString() + " : " + pair.Key, Colour.Red);
            score -= pair.Value;
        }

        Colour pointColour = score >= 0 ? Colour.Blue : Colour.Red;

        summary += totalPoints + ": " + Text(score.ToString(), pointColour) + " / " + maxScore;

        string scoreString = summary + scoreCountPerTask + generalMistakes;
        Logger.Print(scoreString);

        return new Tuple<int, string>(score, scoreString);
    }
}