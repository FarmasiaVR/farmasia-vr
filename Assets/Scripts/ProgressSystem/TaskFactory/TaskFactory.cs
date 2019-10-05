using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class TaskFactory {
    /// <summary>
    /// Static class for creating a new task based on given type.
    /// </summary>
    /// <param name="type">Type given to turn into a Task.</param>
    /// <returns>Returns a new Task based on TaskType.</returns>
    public static ITask GetTask(TaskType type) {
        switch (type) {
            case TaskType.SelectTools:
                return new SelectTools();
            case TaskType.SelectMedicine:
                return new SelectMedicine();
            case TaskType.MedicineToSyringe:
                return new MedicineToSyringe();
            case TaskType.LuerlockAttach:
                return new LuerlockAttach();
            /*case TaskType.AmountOfItems:
                return new AmountOfItems();
            case TaskType.Layout1:
                return new Layout1();
            case TaskType.Layout2:
                return new Layout2();
            case TaskType.MissingItems:
                return new MissingItems();*/
            default:
                return null;
        }
    }
}