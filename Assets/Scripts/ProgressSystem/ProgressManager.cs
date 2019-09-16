using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour {

    public static ProgressManager Instance { get; private set; }
    private TaskFactory taskFactory;
    List<ITask> activeTasks;
    List<ITask> doneTasks;
    private ScoreCalculator calculator;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        taskFactory = new TaskFactory();
        activeTasks = new List<ITask>(); 
        doneTasks = new List<ITask>();       
        AddTasks();
        calculator = new ScoreCalculator();
    }

    private void AddTasks() {
        activeTasks = Enum.GetValues(typeof(TaskType))
            .Cast<TaskType>()
            .Select(v => taskFactory.GetTask(v))
            .ToList();
    }

    public void RemoveTask(ITask task) {
        activeTasks.Remove(task);
        doneTasks.Add(task);
    }

    public List<ITask> GetDoneTasks() {
        return doneTasks;
    }
}
