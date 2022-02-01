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

    /// <summary>
    /// Subtracts a point from given task. Moves it to before time list.
    /// </summary>
    /// <param name="task">Refers to a task to subtract a point.</param>
    public void SubtractBeforeTime(TaskType task, int subtractScore) {
        SubtractPoints(task, subtractScore);
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

    private static readonly Dictionary<Colour, string> ColourCodes = new Dictionary<Colour, string>() {
        { Colour.Yellow, "<color=#ebe134>" },
        { Colour.Black, "<color=#000000>" },
        { Colour.Green, "<color=#2dcc27>" },
        { Colour.Red, "<color=#cc3727>" },
        { Colour.White, "<color=#ffffff>" },
        { Colour.Blue, "<color=#27c7cc>" }
    };

    private string Text(string message, Colour colour) {
        string text = string.Format("{0}{1}</color>", ColourCodes[colour], message);
        return text;
    }

    private string PrintPoints(int gottenPoints, int maxPoints) {
        string text = gottenPoints.ToString();
        text = Text(text,
            gottenPoints < maxPoints ? Colour.Red : Colour.Green
        );
        return string.Format("{0} / {1}", text, Text(maxPoints.ToString(), Colour.Green));
    }

    /// <summary>
    /// Returns current Score for different tasks.
    /// </summary>
    /// <returns>Returns a String presentation of the summary.</returns>
    public void GetScoreString(out int score, out string scoreString, ProgressManager p) {
        string summary = string.Format("Onnittelut {0}, peli päättyi!\n\n", Text(Player.Info.Name, Colour.Blue));
        string scoreCountPerTask = "";
        string addedBeforeTimeList = "";
        string generalMistakes = "\n\nYleisvirheet:\n";
        score = 0;

        foreach (TaskType type in points.Keys) {
            if (maxPoints[type] == 0) {
                continue;
            }
            scoreCountPerTask = string.Format("\n {0} : {1}", PrintPoints(points[type], maxPoints[type]), TaskToString(type));
            if (TaskMistakes.ContainsKey(type)) {
                foreach (string mistake in TaskMistakes[type]) {
                    scoreCountPerTask = string.Format(
                        "{0}\n    {1}",
                        scoreCountPerTask,
                        Text(string.Format("- {0}", mistake), Colour.Red));
                }
            }
            score += points[type];
        }
        foreach (string before in beforeTime) {
            if (beforeTime.Last<string>().Equals(before)) {
                addedBeforeTimeList += before;
                break;
            }
            addedBeforeTimeList += string.Format("{0}, ", before);
        }
        generalMistakes = p.Calculator.Mistakes.Count == 0 ? "" : generalMistakes;
        foreach (var pair in p.Calculator.Mistakes) {
            generalMistakes = string.Format(
                "\n{0} : {1}",
                Text(string.Format("-{0} : {1}", pair.Value.ToString(), pair.Key), Colour.Red)
            );
            score -= pair.Value;
        }

        Colour pointColour = score >= 0 ? Colour.Blue : Colour.Red;

        summary = string.Format(
            "{0}Kokonaispistemäärä: {1} / {2}!\n",
            summary,
            Text(score.ToString(), pointColour),
            maxScore.ToString()
        );
        scoreString = string.Format("{0}{1}{2}", summary, scoreCountPerTask, generalMistakes); // + beforeTimeSummary + addedBeforeTimeList;
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