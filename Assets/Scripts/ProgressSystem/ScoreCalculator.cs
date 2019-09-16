using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour {

    List<String> zero;
    List<String> onePlus;
    List<String> oneMinus;
    private int score;
    private int maxScore;

    private void Start() {
        zero = new List<String>();
        AddTasks();
        onePlus = new List<String>();
        oneMinus = new List<String>();
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