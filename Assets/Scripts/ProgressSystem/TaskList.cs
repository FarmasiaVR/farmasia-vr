using UnityEngine;
using FarmasiaVR.New;
using System.Collections.Generic;
using System;

[Serializable]
[CreateAssetMenu(menuName = "FarmasiaVR/Task List", fileName = "tasks", order = 2)]
public class TaskList : ScriptableObject
{
    public List<Task> tasks;

    private int points;
    private Dictionary<string, Task> taskDict;
    private List<Mistake> generalMistakes;

    private void OnEnable()
    {
        taskDict = new Dictionary<string, Task>();
        // Add every task to a dictionary so that task references are faster and easier.
        // The original task list is still used to track the linear progression of the tasks
        foreach (Task task in tasks)
        {
            if (taskDict.ContainsKey(task.key))
            {
                Debug.LogError("The task list " + this.name + " has multiple tasks with the key " + task.key + ". Please make sure that every task in the task list has a unique key");
                continue;
            }
            taskDict[task.key] = task;
        }
    }

    private void OnValidate()
    {
        ///<summary>
        /// This is a way to show the key of the task as the name or the header of the task in the inspector. Otherwise the tasks would just say "Element N" when collapsed
        /// </summary>
        for(int i = 0; i < tasks.Count; i++)
        {
            Task task = tasks[i];
            task.name = tasks[i].key;
        }
    }

    /// <summary>
    /// Marks the task with the given key as completed and adds the points given by the task to the points counter
    /// </summary>
    /// <param name="taskKey">The key of the task that will be marked as completed. Check the possible keys from the task list.</param>
    /// <returns>A boolean value depending on whether or not the task was succesfully marked as completed</returns>
    public bool MarkTaskAsDone(string taskKey)
    {
        if (!taskDict.ContainsKey(taskKey))
        {
            PrintKeyError(taskKey);
            return false;
        }

        taskDict[taskKey].MarkAsDone();
        points += taskDict[taskKey].awardedPoints;
        return true;
    }

    /// <summary>
    /// Takes a key of a task and returns the Task object related to that key
    /// </summary>
    /// <param name="taskKey">The key of the task to fetch</param>
    /// <returns>Task that has the key given as the parameter</returns>
    public Task GetTask(string taskKey)
    {
        if (!taskDict.ContainsKey(taskKey))
        {
            PrintKeyError(taskKey);
        }

        return taskDict[taskKey];
    }

    public void ResetTaskProgression()
    {
        foreach (Task task in tasks)
        {
            task.Reset();
        }
        points = 0;
    }

    private void PrintKeyError(string taskKey)
    {
        throw new Exception("A task with the key " + taskKey + " could not be found. Make sure that you wrote the key correctly and that you are using the correct task list!");
    }

    public void GenerateTaskMistake(string taskKey, Mistake mistake)
    {
        if (!taskDict.ContainsKey(taskKey))
        {
            PrintKeyError(taskKey);
            return;
        }

        taskDict[taskKey].AddMistake(mistake);
        points -= mistake.pointsDeducted;
    }

    public void GenerateGeneralMistake(Mistake mistake)
    {
        generalMistakes.Add(mistake);
        points -= mistake.pointsDeducted;
    }

}
