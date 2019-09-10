using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTools : TaskData, ITask {

    private void Awake() {
        
    }
    public void finishTask() {
        throw new System.NotImplementedException();
    }

    public string getDescription() {
        return "Valitse sopiva määrä välineitä.";
    }

    public string getHint() {
        throw new System.NotImplementedException();
    }

    public void trigger() {
        throw new System.NotImplementedException();
    }
}
