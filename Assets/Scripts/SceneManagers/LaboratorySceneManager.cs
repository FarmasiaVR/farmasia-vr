using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratorySceneManager : MonoBehaviour
{

    private TaskManager taskManager;
    private GameObject playerEnter;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
        playerEnter = FindObjectOfType<EnterColliders>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindFireExtinguisher()
    {
        if()
        {
            taskManager.CompleteTask("R");

        }
    }

}
