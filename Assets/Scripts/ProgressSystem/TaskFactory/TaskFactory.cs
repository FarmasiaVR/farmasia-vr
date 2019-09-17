using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class TaskFactory {
    public static ITask GetTask(TaskType type) {
        switch (type) {
            case TaskType.SelectTools:
                return new SelectTools();
            //           case TaskType.SelectMedicine:
            //             return new SelectMedicine();
            default:
                return null;
        }
    }
}