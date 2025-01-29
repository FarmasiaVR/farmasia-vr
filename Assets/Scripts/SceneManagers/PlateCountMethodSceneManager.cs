using FarmasiaVR.New;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCountMethodSceneManager : MonoBehaviour
{
    [Tooltip("All items that have to be cleaned and put in a laminar cabinet.")]
    public LaminarCabinetInventory laminarCabinetInventory;

    private TaskManager taskManager;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
        laminarCabinetInventory.Enable();
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

    public void DirtyItem()
    {
        GeneralMistake("Dirty item entered the cabinet", 1);
    }

    public void ItemTouchedTable()
    {
        GeneralMistake("Item touched the table", 1);
    }

    public void GeneralMistake(string message, int penalty)
    {
        taskManager.GenerateGeneralMistake(message, penalty);
    }
}
