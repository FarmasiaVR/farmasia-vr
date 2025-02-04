using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCountMethodSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    private int dilutionTubesAmount = 4500;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
    }

    public void CompleteTask(string taskName)
    {
        Debug.Log($"Trying to complete task");
        taskManager.CompleteTask(taskName);
    }

    public void CleanHands()
    {
        CompleteTask("WashHands");
    }

    public void CheckTubesFill(LiquidContainer container)
    {
        if (container.Amount == dilutionTubesAmount)
        {
            CompleteTask("FillTubes");
        }
    }

    public void GeneralMistake(string message, int penalty)
    {
        taskManager.GenerateGeneralMistake(message, penalty);
    }
}
