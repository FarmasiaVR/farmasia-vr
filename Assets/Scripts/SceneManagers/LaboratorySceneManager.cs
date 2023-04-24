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
        if (iterateThrough())
        {
            IterateFindPlayerEnterScripts();
        }
    }

    /// <summary>
    /// Iterates through PLayerEnter script components present in the Laboratory scene. 
    /// </summary>
    public void IterateFindPlayerEnterScripts()
    {
        foreach (PlayerEnter script in playerEnter)
        {
            if (script.gameObject.name.Contains("PlayerEnterBox FireBlanket") && script.HasEnteredOnce() && !fireBlanketFound)
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

    /// <summary>
    /// Checks, if equipment has been found in the scene.
    /// </summary>
    /// <returns>Returns false if fireBlanketFound, eyeShowerFound, extinguisherFound and emergencyShowerFound are true. Else true.</returns>
    private bool iterateThrough()
    {
        if (fireBlanketFound && eyeShowerFound && extinguisherFound && emergencyShowerFound)
        {
            return false;
        }
        return true;
    }
}
