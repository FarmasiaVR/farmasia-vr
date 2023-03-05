using System.Collections.Generic;
using System.Linq;

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

    private static Dictionary<TaskType, Task> tasks;

    public static void ResetTaskProgression()
    {
        tasks = new List<Task>() {
        // Medicine preparation
        new CorrectItemsInThroughputMedicine(),
        new CorrectItemsInLaminarCabinetMedicine(),
        new DisinfectBottleCap(),
        new MedicineToSyringe(),
        new LuerlockAttach(),
        new SyringeAttach(),
        new CorrectAmountOfMedicineTransferred(),
        new AllSyringesDone(),
        new ItemsToSterileBag(),
        new CleanTrashMedicine(),
        new CorrectItemsInBasketMedicine(),
        new CleanLaminarCabinetMedicine(),
        new FinishMedicine(),

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
        new CloseAgarPlates(TaskType.CloseSettlePlates),
        new WriteSecondTime(),
        new Fingerprints(),
        new CloseAgarPlates(TaskType.CloseFingertipPlates),
        new CloseBottles(),
        new CleanTrashMembrane(),
        new CorrectItemsInBasketMembrane(),
        new CleanLaminarCabinetMembrane(),
        new FinishMembrane(),

        // Changing room
        new WearShoeCoversAndLabCoat(),
        new WashGlasses(),
        new WashHands(PackageName.ChangingRoom, TaskType.WashHandsInChangingRoom),
        new GoToPreperationRoom(),
        new WearHeadCoverAndFaceMask(),
        new WashHands(PackageName.PreperationRoom, TaskType.WashHandsInPreperationRoom),
        new WearSleeveCoversAndProtectiveGloves(),
        new FinishChangingRoom(),

    }.ToDictionary(task => task.TaskType, task => task);
    }
}
