using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratorySceneManager : MonoBehaviour
{

    private TaskManager taskManager;
    private PlayerEnter[] playerEnter;

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
        FindFireExtinguisher();
        FindFireBlanket();
        FindEyeShower();
        FindEmergencyShower();
        //IterateFindTasks();
    }

    public void IterateFindTasks()
    {
        foreach (PlayerEnter script in playerEnter)
        {
            if(script.gameObject.name == "PlayerEnterBox FireBlanket" && script.HasEnteredOnce())
            {
                taskManager.CompleteTask("FireBlanket");
            }
            if (script.gameObject.name == "PlayerEnterBox EyeShower" && script.HasEnteredOnce())
            {
                taskManager.CompleteTask("EyeShower");
            }
            if (script.gameObject.name == "PlayerEnterBox FireExtinguisher" && script.HasEnteredOnce())
            {
                taskManager.CompleteTask("Extinguisher");
            }
            if (script.gameObject.name == "PlayerEnterBox EmergencyShower" && script.HasEnteredOnce())
            {
                taskManager.CompleteTask("EmergencyShower");
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
