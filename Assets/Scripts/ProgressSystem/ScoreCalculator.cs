using System;
using System.Linq;
using System.Collections.Generic;

public class ScoreCalculator {
    #region Fields
    List<string> zero;
    List<string> onePlus;
    List<string> oneMinus;
    private int score;
    private int maxScore;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes ScoreCalculator for point calculation.
    /// </summary>
    public ScoreCalculator() {
        zero = new List<string>();
        onePlus = new List<string>();
        oneMinus = new List<string>();
        score = 0;
        maxScore = 10;
        AddTasks();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Adds all tasks into the list of zero points.
    /// </summary>
    private void AddTasks() {
        zero = Enum.GetValues(typeof(TaskType))
            .Cast<TaskType>()
            .Select(v => v.ToString())
            .ToList();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Adds point to given task. Moves it to plus list.
    /// </summary>
    /// <param name="task">Refers to a task to add a point.</param>
    public void Add(TaskType task) {
        score++;
        onePlus.Add(task.ToString());
        zero.Remove(task.ToString());
    }

    /// <summary>
    /// Substracts a point from given task. Moves it to minus list.
    /// </summary>
    /// <param name="task">Refers to a task to substract a point.</param>
    public void Substract(TaskType task) {
        score--;
        oneMinus.Add(task.ToString());
        zero.Remove(task.ToString());
    }

    /// <summary>
    /// Prints current Score with different tasks.
    /// </summary>
    /// <returns>Returns a String presentation of the summary.</returns>
    public string PrintScore() {
        return "The current score is " + score + " out of " + maxScore + "." + "\t" +
        "Tasks with +1 score: " + String.Join(", ", onePlus.ToArray()) + "\t" +
        "Tasks with -1 score: " + String.Join(", ", oneMinus.ToArray()) + "\t" +
        "Tasks with 0 score: " + String.Join(", ", zero.ToArray());
    }
    #endregion
}