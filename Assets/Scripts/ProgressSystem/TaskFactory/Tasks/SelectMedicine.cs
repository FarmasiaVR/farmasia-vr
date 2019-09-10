using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMedicine : TaskData, ITask  {



    public void Trigger() {
        throw new System.NotImplementedException();
    }

    public void FinishTask() {
        throw new System.NotImplementedException();
    }

    public string GetDescription() {
        finished = true;
        return "TEST";
    }

    public string GetHint() {
        throw new System.NotImplementedException();
    }

    public void NextTask() {
        
    }

}
