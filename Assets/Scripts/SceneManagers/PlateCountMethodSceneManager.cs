using FarmasiaVR.New;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCountMethodSceneManager : MonoBehaviour
{
    [Tooltip("Laminar cabinet that is used to perform actions.")]
    public GameObject cabinet;
    [Tooltip("All items that have to be cleaned and put in a laminar cabinet.")]
    public List<string> itemsToPut = new List<string>();

    private TaskManager taskManager;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
    }
    public void CompleteTask(string taskName)
    {
        taskManager.CompleteTask(taskName);
    }
    public void CleanHands()
    {
        CompleteTask("WashHands");
    }

    public void PutCleanObjects()
    {
        CompleteTask("PutCleanObjects");
    }

    public void PutItem(GeneralItem item)
    {

    }

    public void GeneralMistake(string message, int penalty)
    {
        taskManager.GenerateGeneralMistake(message, penalty);
    }
}
