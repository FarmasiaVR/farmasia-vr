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
    public UnityEvent<Task> onTaskStarted;

    [Tooltip("This is called when a task is completed")]
    public UnityEvent<Task> onTaskCompleted;

    [Tooltip("This is called when the time to finish a task runs out")]
    public UnityEvent<Task> onTaskFailed;

    [Tooltip("This is called when all the tasks in the task list are completed")]
    public UnityEvent<TaskList> onAllTasksCompleted;

    [Tooltip("This is called when a mistake is made")]
    public UnityEvent<Mistake> onMistake;



    private int currentTaskIndex;
    private Task currentTask;

    private void Awake()
    {
        if(resetOnStart)
        {
            taskListObject.ResetTaskProgression();
        }
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
            if (currentTaskIndex > taskListObject.tasks.Count)
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
        currentTask.timeTaskStarted = Time.time;
        if (currentTask.timed && currentTask.failWhenOutOfTime)
        {
            StartCoroutine(TaskCountdown(currentTask.timeToCompleteTask));
        }
    }
    ///<summary>
    ///Marks a task as completed if it hasn't already been completed.
    /// </summary>
    /// <param name="taskKey">The key of the task that should be marked as completed. Check the task list for the possible keys.</param>
    public void CompleteTask(string taskKey)
    {
        if (!taskListObject.MarkTaskAsDone(taskKey))
        {
            return;
        }
        if (currentTask.key == taskKey && currentTask.timed)
        {
            StopCoroutine(TaskCountdown(currentTask.timeToCompleteTask));
        }
        if (taskKey == currentTask.key)
        {
            GetNextTask();
        }
        onTaskCompleted.Invoke((Task)taskListObject.GetTask(taskKey));
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
}
