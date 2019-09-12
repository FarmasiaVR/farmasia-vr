using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TaskFactory {
    public ITask GetTask(TaskType type) {
        switch (type) {
            case TaskType.SelectTools:
                return new SelectTools2();
            //           case TaskType.SelectMedicine:
            //             return new SelectMedicine();
            default:
                return null;
        }
    }
}