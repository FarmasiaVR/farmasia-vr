using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class TaskFactory {
    public static ITask GetTask(TaskType type) {
        switch (type) {
            case TaskType.SelectTools:
                return new SelectTools();
            case TaskType.SelectMedicine:
                return new SelectMedicine();
            case TaskType.Layout1:
                return new Layout1();
            case TaskType.AmountOfItems:
                return new AmountOfItems();
            case TaskType.MissingItems:
                return new MissingItems();
            case TaskType.Layout2:
                return new Layout2();
            default:
                return null;
        }
    }
}