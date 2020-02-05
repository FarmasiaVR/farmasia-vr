using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class ScoreCalculator {

    #region Fields
    Dictionary<TaskType, int> points;
    Dictionary<TaskType, int> maxPoints;
    HashSet<string> beforeTime;
    private int maxScore = 0;

    public Dictionary<string, int> Mistakes { get; private set; }
    public Dictionary<TaskType, HashSet<string>> TaskMistakes { get; private set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes ScoreCalculator for point calculation.
    /// </summary>
    public ScoreCalculator(HashSet<ITask> allTasks) {
        points = new Dictionary<TaskType, int>();
        beforeTime = new HashSet<string>();
        TaskMistakes = new Dictionary<TaskType, HashSet<string>>();
        Mistakes = new Dictionary<string, int>();
        AddTasks(allTasks);
    }
    #endregion

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
    #region Private Methods
    /// <summary>
    /// Adds all tasks into the list of zero points.
    /// </summary>
    private void AddTasks(HashSet<ITask> tasks) {
        maxPoints = new Dictionary<TaskType, int>();
        foreach (ITask task in tasks) {
            points.Add(task.GetTaskType(), task.GetPoints());
            maxScore += task.GetPoints();
            maxPoints.Add(task.GetTaskType(), task.GetPoints());
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Subtracts a point from given task.
    /// </summary>
    /// <param name="task">Refers to a task to subtract a point.</param>
    public void Subtract(TaskType task) {
        if (!points.ContainsKey(task)) {
            return;
        }
        points[task] -= 1;
    }

    /// <summary>
    /// Subtracts a point from given task.
    /// </summary>
    /// <param name="task">Refers to a task to subtract given points.</param>
    /// <param name="subtractScore">Gives the amount of points to be subtracted.</param>
    public void SubtractWithScore(TaskType task, int subtractScore) {
        if (subtractScore < 0) {
            throw new Exception("Cannot subtract negative score!");
        }
        if (!points.ContainsKey(task)) {
            return;
        }
        points[task] -= subtractScore;
    }

    /// <summary>
    /// Subtracts a point from given task. Moves it to before time list.
    /// </summary>
    /// <param name="task">Refers to a task to subtract a point.</param>
    public void SubtractBeforeTime(TaskType task) {
        Subtract(task);
        beforeTime.Add(task.ToString());
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

    private string Text(string message, Colour colour) {
        string text = "";
        switch (colour) {
            case Colour.Black:
                text += "<color=#000000>";
                break;
            case Colour.Yellow:
                text += "<color=#ebe134>";
                break;
            case Colour.Red:
                text += "<color=#cc3727>";
                break;
            case Colour.Green:
                text += "<color=#2dcc27>";
                break;
            case Colour.White:
                text += "<color=#ffffff>";
                break;
            case Colour.Blue:
                text += "<color=#27c7cc>";
                break;
        }
        text += message + "</color>";
        return text;
    }

    private string PrintPoints(int gottenPoints, int maxPoints) {
        string text = "";
        if (gottenPoints < maxPoints) {
            text += Text("" + gottenPoints, Colour.Red);
        } else {
            text += Text("" + gottenPoints, Colour.Green);
        }
        return text += " / " + Text("" + maxPoints, Colour.Green);
    }

    /// <summary>
    /// Returns current Score for different tasks.
    /// </summary>
    /// <returns>Returns a String presentation of the summary.</returns>
    public void GetScoreString(out int score, out string scoreString, ProgressManager p) {

        string summary = "Onnittelut " + Text(Player.Info.Name, Colour.Blue) + ", peli päättyi!\n\n";
        string scoreCountPerTask = "";
        string addedBeforeTimeList = "";
        string taskMistakes = "\n\nTehtävä virheet:\n";
        string generalMistakes = "\n\nYleisvirheet:\n";
        score = 0;

        foreach (TaskType type in points.Keys) {
            if (maxPoints[type] == 0) {
                continue;
            }
            scoreCountPerTask += "\n " + PrintPoints(points[type], maxPoints[type]) + " : " + TaskToString(type);
            if (TaskMistakes.ContainsKey(type)) {
                foreach (string mistake in TaskMistakes[type]) {
                    scoreCountPerTask += "\n  " +Text(mistake, Colour.Red);
                }
            }
            score += points[type];
        }
        foreach (string before in beforeTime) {
            if (beforeTime.Last<string>().Equals(before)) {
                addedBeforeTimeList += before;
                break;
            }
            addedBeforeTimeList += before + ", ";
        }
        generalMistakes = p.Calculator.Mistakes.Count == 0 ? "" : generalMistakes;
        foreach (var pair in p.Calculator.Mistakes) {
            generalMistakes += "\n" + Text("-" + pair.Value + " : " + pair.Key, Colour.Red);
            score -= pair.Value;
        }

        Colour pointColour = score >= 0 ? Colour.Blue : Colour.Red;

        summary += "Kokonaispistemäärä: " + Text("" +  score, pointColour) + " / " + maxScore + "!\n";
        scoreString = summary + scoreCountPerTask + taskMistakes + generalMistakes;// + beforeTimeSummary + addedBeforeTimeList;
        Logger.Print(scoreString);
    }
    private string TaskToString(TaskType type) {
        switch (type) {
            case TaskType.CorrectItemsInThroughput:
                return "Oikeat välineet läpiantokaapissa";
            case TaskType.CorrectLayoutInThroughput:
                return type.ToString();
            case TaskType.CorrectItemsInLaminarCabinet:
                return "Oikeat välineet laminaarikaapissa";
            case TaskType.CorrectLayoutInLaminarCabinet:
                return type.ToString();
            case TaskType.DisinfectBottles:
                return "Pullon korkin desinfiointi";
            case TaskType.MedicineToSyringe:
                return "Lääkkeen otto ruiskuun";
            case TaskType.LuerlockAttach:
                return "Ison ruiskun kiinnitys luerlockiin";
            case TaskType.SyringeAttach:
                return "Pienten ruiskujen kiinnitys luerlockiin";
            case TaskType.CorrectAmountOfMedicineSelected:
                return "Oikea määrä lääkettä ruiskuissa";
            case TaskType.ItemsToSterileBag:
                return "Ruiskujen laittaminen steriilipussiin";
            case TaskType.ScenarioOneCleanUp:
                return "Työskentelytilan siivous";
            default:
                return type.ToString();
        }
    }
    #endregion
}