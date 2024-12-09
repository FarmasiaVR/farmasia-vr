using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FarmasiaVR.New;
using System.Collections;

public class TaskDescriptionManagerPCM : MonoBehaviour // copied TaskDescriptionManager for PCM scene
{
    [Tooltip("The list of task descriptions that the manager will update. Leave empty to find the task descriptions by tag.")]
    public List<TextMeshPro> taskDescriptions;

    private TaskList currentTaskList;
    private Task currentTask;

    private void Awake()
    {
        taskDescriptions = new List<TextMeshPro>(); // Initialize the list
    }

    private IEnumerator Start()
    {
        // Wait for VR or Unity systems to initialize
        while (!AreDependenciesReady())
        {
            Debug.Log("Waiting for dependencies...");
            yield return null;
        }

        Debug.Log("Dependencies are ready, updating task descriptions.");
        UpdateTaskDescriptionList();
    }

    private bool AreDependenciesReady()
    {
        // Ensure at least one "TaskDescription" object exists
        return GameObject.FindWithTag("TaskDescription") != null;
    }

    private void OnEnable()
    {
        LocalSelector.OnLocaleChanged += UpdateGameOverText;
        TaskEventManagerPCM.OnTaskStarted += UpdateTaskDescriptions;
    }

    private void OnDisable()
    {
        LocalSelector.OnLocaleChanged -= UpdateGameOverText;
        TaskEventManagerPCM.OnTaskStarted -= UpdateTaskDescriptions;
    }

    public void UpdateTaskDescriptionList()
    {
        taskDescriptions.Clear(); // Empty the list

        GameObject[] descObjects = GameObject.FindGameObjectsWithTag("TaskDescription");
        Debug.Log($"Found {descObjects.Length} TaskDescription objects.");

        if (descObjects.Length == 0)
        {
            Debug.LogWarning("No TaskDescription objects found!");
            return;
        }

        foreach (GameObject descObject in descObjects)
        {
            if (!descObject.activeInHierarchy)
            {
                Debug.LogWarning($"Inactive TaskDescription object: {descObject.name}");
                continue;
            }

            TextMeshPro textComponent = descObject.GetComponent<TextMeshPro>();
            if (textComponent != null)
            {
                taskDescriptions.Add(textComponent);
                if (currentTask != null)
                {
                    textComponent.text = GetTranslatedTaskText(currentTask.name);
                }
            }
        }
    }

    public void UpdateTaskDescriptions(Task task)
    {
        if (task == null)
        {
            Debug.LogError("UpdateTaskDescriptions called with null task!");
            return;
        }

        currentTask = task;
        Debug.Log($"Updating task descriptions for task: {task.name}");

        if (taskDescriptions == null || taskDescriptions.Count == 0)
        {
            Debug.LogWarning("No task descriptions available to update!");
            return;
        }

        foreach (TextMeshPro taskDesc in taskDescriptions)
        {
            if (currentTask != null)
            {
                taskDesc.text = GetTranslatedTaskText(currentTask.name);
            }
        }
    }

    public void GameOverText(TaskList taskList)
    {
        currentTaskList = taskList;
        UpdateTaskDescriptionsWithPoints();
    }

    private void UpdateGameOverText()
    {
        UpdateTaskDescriptionList();
        if (currentTaskList != null)
        {
            UpdateTaskDescriptionsWithPoints();
        }
    }

    private void UpdateTaskDescriptionsWithPoints()
    {
        if (currentTaskList == null)
        {
            Debug.LogWarning("CurrentTaskList is null, cannot update points!");
            return;
        }

        foreach (TextMeshPro taskDesc in taskDescriptions)
        {
            string translatedText = Translator.Translate("PlateCountMethod", "MissionCompleted") + " " + currentTaskList.GetPoints();
            taskDesc.text = translatedText;
        }
    }

    public int getValue()
    {
        if (currentTaskList == null)
        {
            Debug.LogError("CurrentTaskList is null, cannot get value!");
            return 0;
        }
        return currentTaskList.GetPoints();
    }

    private string GetTranslatedTaskText(string taskName)
    {
        if (string.IsNullOrEmpty(taskName))
        {
            Debug.LogError("Task name is null or empty!");
            return "Unknown Task";
        }
        string translatedText = Translator.Translate("PlateCountMethod", taskName);
        if (string.IsNullOrEmpty(translatedText))
        {
            Debug.LogError($"Translation missing for task: {taskName}");
            return $"[Untranslated: {taskName}]";
        }

        return translatedText;
    }
}

// Simulated TaskEventManager class to manage events
public static class TaskEventManagerPCM
{
    public static event System.Action<Task> OnTaskStarted;

    public static void TriggerTaskStarted(Task task)
    {
        if (task == null)
        {
            Debug.LogError("TriggerTaskStarted called with null task!");
            return;
        }

        Debug.Log($"Task started: {task.name}");
        OnTaskStarted?.Invoke(task);
    }
}
