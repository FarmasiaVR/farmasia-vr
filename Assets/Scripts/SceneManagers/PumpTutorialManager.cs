using FarmasiaVR.New;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpTutorialManager : MonoBehaviour
{
    private TaskManager taskManager;
    private bool filterConnectedToPump;
    public Blade scalpel;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();

        filterHalfInSoycaseine = false;
        filterHalfInTioglycolate = false;
    }


    /// <summary>
    /// Call this when starting a new task. If you need some logic when starting specific tasks, write them here.
    /// </summary>
    /// <param name="startedTask"></param>
    public void TaskStartEvents(Task startedTask)
    {
        // Make sure that the player can only cut the filter when it is time.
        if (startedTask.key == "cutFilter")
        {
            scalpel.enabled = true;
        }
    }
    #region Filter events
    public void SetFilterConnectionStatus(bool value)
    {
        filterConnectedToPump = value;
    }

    /// <summary>
    /// Call this when the liquid container in the pump is filled
    /// </summary>
    /// <param name="liquidContainer"></param>
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
    #endregion

    #region Pump events
    /// <summary>
    /// Call this when the pump is turned on
    /// </summary>
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

    /// <summary>
    /// Call this when the filtermiddle part is removed from its socket. Makes sure that the task can only be completed when it's the active task
    /// </summary>
    public void CheckPumpDisassemble()
    {
        if (taskManager.GetCurrentTask().key == "pumpDisassemble")
        {
            taskManager.CompleteTask("pumpDisassemble");
        }
    }
    #endregion

    #region Filter half events

    private bool filterHalfInSoycaseine = false;
    private bool filterHalfInTioglycolate = false;

    public void NotifyFilterHalfInWrongBottle(GeneralItem genItem)
    {
        taskManager.GenerateTaskMistake("Laitoit suodattimen puolikkaan v‰‰r‰‰n pulloon", 1);
        genItem.transform.Translate(new Vector3(0.05f, 0, 0));
    }

    public void FilterHalfInSoycaseine(GeneralItem genItem)
    {
        if (filterHalfInSoycaseine) {NotifyFilterHalfInWrongBottle(genItem);}

        else
        {
            filterHalfInSoycaseine = true;
            if (filterHalfInTioglycolate)
            {
                CompleteFilterHalfTask();
            }
        }
    }

    public void FilterHalfInTioglycolate(GeneralItem genItem)
    {
        if (filterHalfInTioglycolate) { NotifyFilterHalfInWrongBottle(genItem); }

        else
        {
            filterHalfInTioglycolate = true;
            if (filterHalfInSoycaseine)
            {
                CompleteFilterHalfTask();
            }
        }
    }

    private void CompleteFilterHalfTask()
    {
        taskManager.CompleteTask("halvesToBottles");
    }
    #endregion
}
