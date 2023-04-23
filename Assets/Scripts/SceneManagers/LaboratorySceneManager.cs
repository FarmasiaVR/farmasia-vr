using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratorySceneManager : MonoBehaviour
{

    private TaskManager taskManager;
    private TaskboardManager taskboardManager;
    private PlayerEnter[] playerEnter;
    private bool fireBlanketFound, eyeShowerFound, extinguisherFound, emergencyShowerFound;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
        taskboardManager = GetComponent<TaskboardManager>();
        playerEnter = FindObjectsOfType<PlayerEnter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //FindFireExtinguisher();
        //FindFireBlanket();
        //FindEyeShower();
        //FindEmergencyShower();
        IterateFindTasks();
    }

    public void IterateFindTasks()
    {
        foreach (PlayerEnter script in playerEnter)
        {
            if(script.gameObject.name.Contains("PlayerEnterBox FireBlanket") && script.HasEnteredOnce() && !fireBlanketFound)
            {
                taskboardManager.MarkTaskAsCompleted("FireBlanket");
                taskManager.CompleteTask("FireBlanket");
                fireBlanketFound = true;
            }
            if (script.gameObject.name.Contains("PlayerEnterBox EyeShower") && script.HasEnteredOnce() && !eyeShowerFound)
            {
                taskboardManager.MarkTaskAsCompleted("EyeShower");
                taskManager.CompleteTask("EyeShower");
                eyeShowerFound = true;
            }
            if (script.gameObject.name.Contains("PlayerEnterBox FireExtinguisher") && script.HasEnteredOnce() && !extinguisherFound)
            {
                taskboardManager.MarkTaskAsCompleted("Extinguisher");
                taskManager.CompleteTask("Extinguisher");
                extinguisherFound = true;
            }
            if (script.gameObject.name.Contains("PlayerEnterBox EmergencyShower") && script.HasEnteredOnce() && !emergencyShowerFound)
            {
                taskboardManager.MarkTaskAsCompleted("EmergencyShower");
                taskManager.CompleteTask("EmergencyShower");
                emergencyShowerFound = true;
            }
        }
    }

    public void FindFireBlanket()
    {
        if (playerEnter[0].HasEnteredOnce())
        {
            //Debug.Log("Task complete(?)");
            taskManager.CompleteTask("FireBlanket");
        }
    }

    public void FindFireExtinguisher()
    {
        if (playerEnter[1].HasEnteredOnce())
        {
            //Debug.Log("Task complete(?)");
            taskManager.CompleteTask("Extinguisher");
        }
    }

    public void FindEyeShower()
    {
        if (playerEnter[2].HasEnteredOnce())
        {
            //Debug.Log("Task complete(?)");
            taskManager.CompleteTask("EyeShower");
        }
    }

    public void FindEmergencyShower()
    {
        if (playerEnter[3].HasEnteredOnce())
        {
            //Debug.Log("Task complete(?)");
            taskManager.CompleteTask("EmergencyShower");
        }
    }

   

}
