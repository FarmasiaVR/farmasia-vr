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
            default:
                return null;
        }
    }
}