using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FarmasiaVR.New;

public class TaskDescriptionManager : MonoBehaviour
{
    [Tooltip("The list of task descriptions that the manager will update. Leave empty to find the task descriptions by tag.")]
    public List<TextMeshPro> taskDescriptions;

    private TaskList currentTaskList;
    private Task currentTask;

    private void Awake()
    {
        taskDescriptions = new List<TextMeshPro>(); // Create the list
    }

    private IEnumerator Start()
    {
        // Wait for Unity and VR systems to initialize
        yield return null; // Wait one frame for initialization
        yield return new WaitForSeconds(0.5f); // Optionally add more delay for VR setup

        UpdateTaskDescriptionList();
    }

    private void OnEnable()
    {
        LocalSelector.OnLocaleChanged += UpdateGameOverText;
    }

    private void OnDisable()
    {
        LocalSelector.OnLocaleChanged -= UpdateGameOverText;
    }

    public void UpdateTaskDescriptionList()
    {
        taskDescriptions.Clear(); // Empty the list

        // Find all objects tagged as "TaskDescription"
        GameObject[] descObjects = GameObject.FindGameObjectsWithTag("TaskDescription");
        if (descObjects.Length == 0)
        {
            Debug.LogWarning("No TaskDescription objects found!");
            return;
        }

        foreach (GameObject descObject in descObjects)
        {
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
        currentTask = task;

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
        foreach (TextMeshPro taskDesc in taskDescriptions)
        {
            string translatedText = Translator.Translate("LaboratoryTour", "MissionCompleted") + " " + currentTaskList.GetPoints();
            taskDesc.text = translatedText;
        }
    }

    public int getValue()
    {
        return currentTaskList.GetPoints();
    }

    private string GetTranslatedTaskText(string taskName)
    {
        if (taskName == "EmergencyShower" || taskName == "Extinguisher" || taskName == "FireBlanket" ||
            taskName == "EyeShower" || taskName == "EmergencyExit")
        {
            return Translator.Translate("LaboratoryTour", taskName);
        }
        else
        {
            return Translator.Translate("ControlsTutorial", taskName);
        }
    }
}
