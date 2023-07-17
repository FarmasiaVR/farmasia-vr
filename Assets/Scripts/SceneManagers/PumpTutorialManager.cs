using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpTutorialManager : MonoBehaviour
{
    private TaskManager taskManager;
    private bool filterConnectedToPump;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
    }

    public void SetFilterConnectionStatus(bool value)
    {
        filterConnectedToPump = value;
    }

    public void CheckFilterFill(LiquidContainer liquidContainer)
    {
        if (liquidContainer.LiquidType == LiquidType.Peptonwater && liquidContainer.Amount >= 1000)
        {
            taskManager.CompleteTask("fillPepton");
        }

        else if (liquidContainer.LiquidType == LiquidType.Medicine && liquidContainer.Amount >= 150 && taskManager.GetCurrentTask().key == "fillMedicine")
        {
            taskManager.CompleteTask("fillMedicine");
        }
    }

    public void CheckPumpUsage()
    {
        if (!filterConnectedToPump) { return; }

        if (taskManager.GetCurrentTask().key == "usePump")
        {
            taskManager.CompleteTask("usePump");
        }

        else if (taskManager.GetCurrentTask().key == "usePumpAgain")
        {
            taskManager.CompleteTask("usePumpAgain");
        }
    }
}
