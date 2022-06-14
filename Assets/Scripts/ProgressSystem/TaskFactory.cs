using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class TaskFactory {
    /// <summary>Get a task by type and call subscribe on it, effectively activating it
    /// </summary>
    public static Task GetTask(TaskType type) {
        if (tasks.ContainsKey(type)) {
            var task = tasks[type];
            task.Subscribe();
            return task;
        } else {
            Logger.Error("Unknown TaskType " + type);
            return null;
        }
    }

    private static Dictionary<TaskType, Task> tasks = new List<Task>() {
        // Medicine preparation
        new SelectTools(),
        new SelectMedicine(),
        new CorrectItemsInThroughput(),
        new CorrectLayoutInThroughput(),
        new CorrectItemsInLaminarCabinet(),
        new CorrectLayoutInLaminarCabinet(),
        new DisinfectBottles(),
        new MedicineToSyringe(),
        new LuerlockAttach(),
        new SyringeAttach(),
        new CorrectAmountOfMedicineSelected(),
        new ItemsToSterileBag(),
        new ScenarioOneCleanUp(),
        new Finish(),

        // Membrane filtration
        new SelectToolsMembrane(),
        new CorrectItemsInThroughputMembrane(),
        new CorrectItemsInLaminarCabinetMembrane(),
        new WriteTextsToItems(),
        new OpenAgarplates(),
        new FillBottles(),
        new AssemblePump(),
        new LiquidToFilter("Lisää peptonivesi suodattimeen", 1000, LiquidType.Peptonwater, TaskType.WetFilter),
        new StartPump(TaskType.StartPump),
        new LiquidToFilter("Lisää lääke suodattimeen", 150, LiquidType.Medicine, TaskType.MedicineToFilter),
        new StartPump(TaskType.StartPumpAgain),
        new CutFilter(),
        new FilterHalvesToBottles(),
        new CloseAgarplates(),
        new Fingerprints(),
        new FinishMembrane(),
        new WriteSecondTime(),

        // ChangingRoom
        new WashHands(),

    }.ToDictionary(task => task.TaskType, task => task);
}