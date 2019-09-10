using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour {
    public static ProgressManager Instance { get; private set; }

    private TaskFactory taskFactory;

    public ITask currentTask;



    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        taskFactory = new TaskFactory();
        currentTask = taskFactory.GetTask(TaskType.SelectTools);

    }

}
