using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyShowerTutorialSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    private TaskboardManager taskboardManager;
    private bool playerExtinguished, doneOnce;
    
    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
        taskboardManager = GetComponent<TaskboardManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(CheckPlayerExtinguished() && !doneOnce)
        {
            taskManager.CompleteTask("PlayerOnFire");
            doneOnce = true;
        }
    }

    private bool CheckPlayerExtinguished()
    {
        return playerExtinguished;
    }

    public void SetPlayerExtinguished()
    {
        Debug.Log("Setting boolean values in SetPlayerExtinguished!");
        playerExtinguished = true;
    }

}

