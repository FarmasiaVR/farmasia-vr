using System;
using System.Linq;
using System.Collections.Generic;

public class ScoreCalculator {

    #region fields
    private List<string> zero;
    private List<string> onePlus;
    private List<string> oneMinus;
    private int score;
    private int maxScore;
    #endregion

    public ScoreCalculator() {
        zero = new List<string>();
        AddTasks();
        onePlus = new List<string>();
        oneMinus = new List<string>();
        score = 0;
        maxScore = 10;
    }

    private void AddTasks() {
        zero = Enum.GetValues(typeof(TaskType))
            .Cast<TaskType>()
            .Select(v => v.ToString())
            .ToList();
    }

    public void Add(TaskType task) {
        score++;
        onePlus.Add(task.ToString());
        zero.Remove(task.ToString());
    }

    public void Substract(TaskType task) {
        score--;
        oneMinus.Add(task.ToString());
        zero.Remove(task.ToString());
    }

    public string PrintScore() {
        return "The current score is " + score + " out of " + maxScore + "." + "\t" +
        "Tasks with +1 score: " + String.Join(", ", onePlus.ToArray()) + "\t" +
        "Tasks with -1 score: " + String.Join(", ", oneMinus.ToArray()) + "\t" +
        "Tasks with 0 score: " + String.Join(", ", zero.ToArray());
    }
}