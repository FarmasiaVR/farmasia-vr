using System;
using System.Linq;
using System.Collections.Generic;

public class ScoreCalculator {

    #region Fields
    Dictionary<TaskType, int> points;
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
        foreach (ITask task in tasks) {
            points.Add(task.GetTaskType(), task.GetPoints());
            maxScore += task.GetPoints();
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
        String summary = "Peli päättyi - onnittelut!\n";
        String scoreCountPerTask = "";
        String beforeTimeSummary = "Liian aikaisin koitetut tehtävät:\n";
        String addedBeforeTimeList = "";
        score = 0;

        foreach (TaskType type in points.Keys) {
            scoreCountPerTask += "\n Tehtävä:" + type.ToString() + ": " + points[type] + "pistettä.";
            score += points[type];
        }
        foreach (String before in beforeTime) {
            if (beforeTime.Last<String>().Equals(before)) {
                addedBeforeTimeList += before;
                break;
            }
            addedBeforeTimeList += before + ", ";
        }
        summary += "Kokonaispistemäärä: " + score + "/" + maxScore + "!";
        Logger.Print(summary + scoreCountPerTask + beforeTimeSummary + addedBeforeTimeList);
        scoreString = summary + scoreCountPerTask + beforeTimeSummary + addedBeforeTimeList;
    }
    #endregion
}