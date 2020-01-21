using System;
using System.Linq;
using System.Collections.Generic;

public class ScoreCalculator {

    #region Fields
    Dictionary<TaskType, int> points;
    Dictionary<TaskType, int> maxPoints;
    HashSet<ITask> tasks;
    HashSet<String> beforeTime;
    private int maxScore = 0;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes ScoreCalculator for point calculation.
    /// </summary>
    public ScoreCalculator(HashSet<ITask> allTasks) {
        points = new Dictionary<TaskType, int>();
        tasks = new HashSet<ITask>(allTasks);
        beforeTime = new HashSet<String>();
        AddTasks();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Adds all tasks into the list of zero points.
    /// </summary>
    private void AddTasks() {
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

    /// <summary>
    /// Returns current Score for different tasks.
    /// </summary>
    /// <returns>Returns a String presentation of the summary.</returns>
    public void GetScoreString(out int score, out string scoreString) {
        string summary = "Onnittelut " + Player.Info.Name + ", peli päättyi!\n\n";
        string scoreCountPerTask = "";
        string beforeTimeSummary = "Liian aikaisin koitetut tehtävät:\n";
        string addedBeforeTimeList = "";
        score = 0;

        foreach (TaskType type in points.Keys) {
            if (maxPoints[type] == 0) {
                continue;
            }
            scoreCountPerTask += "\n " + points[type] + " / " + maxPoints[type] + " : " + type.ToString();
            score += points[type];
        }
        foreach (string before in beforeTime) {
            if (beforeTime.Last<string>().Equals(before)) {
                addedBeforeTimeList += before;
                break;
            }
            addedBeforeTimeList += before + ", ";
        }
        summary += "Kokonaispistemäärä: " + score + " / " + maxScore + "!";
        scoreString = summary + scoreCountPerTask;// + beforeTimeSummary + addedBeforeTimeList;
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