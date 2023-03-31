using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FarmasiaVR.New;

public class TaskDescriptionManager : MonoBehaviour
{
    private List<TextMeshPro> taskDescriptions;

    private void Awake()
    {
        taskDescriptions = new List<TextMeshPro>();
        foreach (GameObject descObject in GameObject.FindGameObjectsWithTag("TaskDescription"))
        {
            taskDescriptions.Add(descObject.GetComponent<TextMeshPro>());
        }
    }

    /// <summary>
    /// Updates the task descriptions in the scene with new info
    /// </summary>
    /// <param name="task">The task that will have its info shows in the scene</param>
    public void UpdateTaskDescriptions(Task task)
    {
        foreach (TextMeshPro taskDesc in taskDescriptions)
        {
            taskDesc.text = task.taskText;
        }
    }
    /// <summary>
    /// Updates all the descriptions to show how many points the player has collected.
    /// Used for testing and shouldn't be used in the final game
    /// </summary>
    /// <param name="taskList">The task list the player has finished</param>
    public void GameOverText(TaskList taskList)
    {
        foreach (TextMeshPro taskDesc in taskDescriptions)
        {
            taskDesc.text = "Kaikki tehtävät suoritettu! \n Sait yhteensä " + taskList.GetPoints() + " pistettä";
        }
    }
}
