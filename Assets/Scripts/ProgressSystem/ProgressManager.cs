using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour {

    public static ProgressManager Instance { get; private set; }
    private TaskFactory taskFactory;
    List<ITask> tasks;
    private int progressPointer;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        taskFactory = new TaskFactory();
        tasks = new List<ITask>();        
        AddTask(TaskType.SelectTools);
        progressPointer = 0;
    }

    public void AddTask(TaskType task) {
        tasks.Add(taskFactory.GetTask(task));
    }

    public void AddMultipleTasks(TaskType[] taskTypes) {
        foreach (TaskType taskType in taskTypes) {
            tasks.Add(taskFactory.GetTask(taskType));            
        }
    }

    public void MovePointer() {
        if (progressPointer < tasks.Count) {
            progressPointer++;
        }
    }

    

    

    



    

}
