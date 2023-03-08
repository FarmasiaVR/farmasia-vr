using UnityEngine;
using FarmasiaVR.New;
using System.Collections.Generic;
using System;

[Serializable]
[CreateAssetMenu(menuName = "FarmasiaVR/Task List", fileName = "tasks", order = 2)]
public class TaskList : ScriptableObject
{
    public List<Task> tasks;
    private float points { get { return points; } }
    private Dictionary<String, Task> taskDict;

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

    private void OnEnable()
    {
        taskDict= new Dictionary<String, Task>();
        // Add every task to a dictionary so that task references are faster and easier.
        // The original task list is still used to track the linear progression of the tasks
        foreach(Task task in tasks)
        {
            taskDict[task.key] = task;
        }
    }
}
