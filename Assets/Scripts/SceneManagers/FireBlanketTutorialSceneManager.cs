using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlanketTutorialSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    public FireGrid fireGridObj;

    // Start is called before the first frame update
    void Start()
    {
        taskManager = GetComponent<TaskManager>();
        fireGridObj = GetComponent<FireGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeBlanket()
    {
        taskManager.CompleteTask("TakeBlanket");
    }

    public void OpenBlanket()
    {
        taskManager.CompleteTask("OpenBlanket");
    }

    public void ExtinguishFire()
    {
        taskManager.CompleteTask("ExtinguishFire");
    }
}
