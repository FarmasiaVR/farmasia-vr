using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FarmasiaVR.New;
using UnityEngine.Events;

public class TaskManager : MonoBehaviour
{
    [Tooltip("The task list that is used in the current scene.")]
    public TaskList taskListObject;

    [Tooltip("Whether the task progression should be reset when the scene is loaded")]
    public bool resetOnStart;

    [Header("Task events")]

    [Tooltip("This is called when the active task changes")]
    public UnityEvent<Task> onTaskStarted = new UnityEvent<Task>();

    [Tooltip("This is called when a task is completed")]
    public UnityEvent<Task> onTaskCompleted = new UnityEvent<Task>();

    [Tooltip("This is called when the time to finish a task runs out")]
    public UnityEvent<Task> onTaskFailed = new UnityEvent<Task>();

    [Tooltip("This is called when all the tasks in the task list are completed")]
    public UnityEvent<TaskList> onAllTasksCompleted = new UnityEvent<TaskList>();

    [Tooltip("This is called when a mistake is made")]
    public UnityEvent<Mistake> onMistake = new UnityEvent<Mistake>();



    private int currentTaskIndex;
    private Task currentTask;
    private Coroutine timerCoroutine; 

    private void Start()
    {
        if(resetOnStart)
        {
            taskListObject.ResetTaskProgression();
        }
        GetNextTask();
    }


    ///<summary>
    ///Gets the next task from the task list that isn't completed. In other words, gets the first task from the task list that hasn't been completed.
    /// </summary>
    private void GetNextTask()
    {
        bool newTaskFound = false;
        while (!newTaskFound)
        {
            /// If we have reached the end of the task list, that means all of the tasks have been completed.
            if (currentTaskIndex >= taskListObject.tasks.Count)
            {
                onAllTasksCompleted.Invoke(taskListObject);
                return;
            }
            /// Iterate through the task list starting from the index of the currently active task until we find one that hasn't been completed.
            if (!taskListObject.tasks[currentTaskIndex].completed)
            {
                currentTask = taskListObject.tasks[currentTaskIndex];
                newTaskFound = true;
            }
            currentTaskIndex++;
        }
        onTaskStarted.Invoke(currentTask);
        currentTask.StartTaskTimer();
        if (currentTask.timed && currentTask.failWhenOutOfTime)
        {
            timerCoroutine = StartCoroutine(TaskCountdown(currentTask.timeToCompleteTask));
        }
    }
    ///<summary>
    ///Marks a task as completed if it hasn't already been completed.
    /// </summary>
    /// <param name="taskKey">The key of the task that should be marked as completed. Check the task list for the possible keys.</param>
    public void CompleteTask(string taskKey)
    {
        if (IsTaskCompleted(taskKey)) { return; }

        if (taskKey != currentTask.key && GetTask(taskKey).timed)
        {
            Debug.LogWarning("You are completing task " + taskKey + " that is timed but isn't active. Make sure that you only complete timed tasks when they are active!");
        }
        taskListObject.MarkTaskAsDone(taskKey);

        if (currentTask.key == taskKey && currentTask.timed && currentTask.failWhenOutOfTime)
        {
            StopCoroutine(timerCoroutine);
        }

        onTaskCompleted.Invoke((Task)taskListObject.GetTask(taskKey));

        if (taskKey == currentTask.key)
        {
            GetNextTask();
        }
    }


    /// <summary>
    /// This is used to track time sensitive tasks. Sends a signal if the task isn't completed in time.
    /// </summary>
    /// <param name="timeToCompleteTask">The time it takes for the countdown to send the task failed signal</param>
    /// <returns></returns>
    private IEnumerator TaskCountdown(float timeToCompleteTask)
    {
        yield return new WaitForSeconds(timeToCompleteTask);
        onTaskFailed.Invoke(currentTask);
    }

    /// <summary>
    /// Adds a mistake to the list of the mistakes done to the currently active task and deducts the score from the accumulated points.
    /// </summary>
    /// <param name="mistakeText">What the mistake is</param>
    /// <param name="deductedPoints">How many points should be deducted for the mistake</param>
    public void GenerateTaskMistake(string mistakeText, int deductedPoints)
    {
        Mistake mistake = new Mistake(mistakeText, deductedPoints);
        taskListObject.GenerateTaskMistake(currentTask.key, mistake);
        onMistake.Invoke(mistake);
    }

    /// <summary>
    /// Adds a mistake to the list of general mistakes and deducts the score from the accumulated points.
    /// </summary>
    /// <param name="mistakeText">What the mistake is</param>
    /// <param name="deductedPoints">How many points should be deducted for the mistake</param>
    public void GenerateGeneralMistake(string mistakeText, int deductedPoints)
    {
        Mistake mistake = new Mistake(mistakeText, deductedPoints);
        taskListObject.GenerateGeneralMistake(mistake);
        onMistake.Invoke(mistake);
    }
    /// <summary>
    /// </summary>
    /// <returns>The task that is currently active</returns>
    public Task GetCurrentTask() {
        return currentTask;
    }
    /// <summary>
    /// Returns whether or not a task has been completed
    /// </summary>
    /// <param name="taskKey">The key of the task</param>
    /// <returns>True or false depending on if the task has been marked as completed.</returns>
    public bool IsTaskCompleted(string taskKey)
    {
        return taskListObject.GetTask(taskKey).completed;
    }
    /// <summary>
    /// </summary>
    /// <param name="taskKey">The key of the task</param>
    /// <returns>The task that has taskKey as its key</returns>
    public Task GetTask(string taskKey)
    {
        return taskListObject.GetTask(taskKey);
    }
}
