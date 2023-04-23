using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FarmasiaVR.New;

public class TaskboardManager : MonoBehaviour
{
    private List<Taskboard> taskboards = new List<Taskboard>();

    private void Awake()
    {
        foreach (GameObject taskboard in GameObject.FindGameObjectsWithTag("Taskboard"))
        {
            taskboards.Add(taskboard.GetComponent<Taskboard>());
        }
    }

    public void InitTaskboards(TaskList tasklist)
    {
        foreach(Taskboard taskboard in taskboards) 
        {
            taskboard.InitTaskboard(tasklist);
        }
    }

    public void MarkTaskAsCompleted(string taskKey)
    {
        foreach(Taskboard taskboard in taskboards)
        {
            taskboard.MarkTaskAsCompleted(taskKey);
        }
    }
}
