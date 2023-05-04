using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTutorialManager : MonoBehaviour
{
    private TaskManager taskManager;

    /// <summary>
    /// This a scene manager used for the controls tutorial scene. This is used to keep track of the progression in the scene.
    /// </summary>
    private void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }
    public void CheckBottleFill(int amountInBottle)
    {
        if (amountInBottle >= 30000)
        {
            taskManager.CompleteTask("med");
        }
    }
}
