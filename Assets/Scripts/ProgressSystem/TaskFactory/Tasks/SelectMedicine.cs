using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMedicine : TaskData, ITask  {


    private void Awake() {
        
    }

    /// <summary>
    /// When 
    /// </summary>
    public void trigger() {
        throw new System.NotImplementedException();
    }

    public void finishTask() {
        throw new System.NotImplementedException();
    }

    public string getDescription() {
        finished = true;
        return "TEST";
    }

    public string getHint() {
        throw new System.NotImplementedException();
    }

}
