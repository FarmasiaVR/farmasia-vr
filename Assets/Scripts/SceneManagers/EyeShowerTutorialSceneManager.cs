using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeShowerTutorialSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    // Start is called before the first frame update
    void Start()
    {
        taskManager = GetComponent<TaskManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PickUp()
    {
        taskManager.CompleteTask("Pickup");
    }

    public void Activate()
    {
        taskManager.CompleteTask("Activate");
    }

    public void Aim()
    {
        if (taskManager.IsTaskCompleted("Activate"))
        {
            taskManager.CompleteTask("Aim");
        }
    }
}

