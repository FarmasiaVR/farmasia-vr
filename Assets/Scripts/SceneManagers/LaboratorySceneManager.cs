using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratorySceneManager : MonoBehaviour
{

    private TaskManager taskManager;
    private PlayerEnter[] playerEnter;
    private bool fireBlanketFound, eyeShowerFound, extinguisherFound, emergencyShowerFound;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
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
                taskManager.CompleteTask("FireBlanket");
                fireBlanketFound = true;
            }
            if (script.gameObject.name.Contains("PlayerEnterBox EyeShower") && script.HasEnteredOnce() && !eyeShowerFound)
            {
                taskManager.CompleteTask("EyeShower");
                eyeShowerFound = true;
            }
            if (script.gameObject.name.Contains("PlayerEnterBox FireExtinguisher") && script.HasEnteredOnce() && !extinguisherFound)
            {
                taskManager.CompleteTask("Extinguisher");
                extinguisherFound = true;
            }
            if (script.gameObject.name.Contains("PlayerEnterBox EmergencyShower") && script.HasEnteredOnce() && !emergencyShowerFound)
            {
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
