using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSceneManager : MonoBehaviour
{
    private TaskManager taskManager;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PressQ();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            PressW();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            PressE();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            PressR();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            PressT();
        }
    }


    public void PressQ()
    {
        taskManager.CompleteTask("Q");
    }

    public void PressW()
    {
        taskManager.CompleteTask("W");
    }

    public void PressE()
    {
        if(!taskManager.IsTaskCompleted("Q") || !taskManager.IsTaskCompleted("W"))
        {
            taskManager.GenerateTaskMistake("You have to press Q and W before pressing E", 5);
            return;
        }

        taskManager.CompleteTask("E");
    }

    public void PressR()
    {
        ///If pressing R isn't the current task, generate a general mistake
        if (taskManager.GetCurrentTask().key != "R")
        {
            taskManager.GenerateGeneralMistake("Make sure to press Q, W and E before pressing R", 2);
        }
        taskManager.CompleteTask("R");
    }

    public void PressT()
    {
        if (!taskManager.IsTaskCompleted("R"))
        {
            taskManager.GenerateGeneralMistake("Make sure to press Q, W, E and R before pressing T", 3);
        }
    }

}
