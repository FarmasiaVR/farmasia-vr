using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FarmasiaVR.New;

public class TaskDescriptionManager : MonoBehaviour
{
    [Tooltip("The list of task descriptions that the manager will update. Leave empty to find the task descriptions by tag.")]
    public List<TextMeshPro> taskDescriptions;

    private TaskList currentTaskList;

    private void Awake()
    {
        taskDescriptions = new List<TextMeshPro>();  // Create the list
    }

    private void Start()
    {
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
        taskDescriptions.Clear();  // Empty the list
        
        // Find all objects which have the tag "TaskDescription"
        foreach (GameObject descObject in GameObject.FindGameObjectsWithTag("TaskDescription"))
        {
            TextMeshPro textComponent = descObject.GetComponent<TextMeshPro>();
            if (textComponent != null)
            {
                taskDescriptions.Add(textComponent);
                textComponent.text = "This should not be shown";
            }
        }
        
    }

      public void UpdateTaskDescriptions(Task task)
    {
        foreach (TextMeshPro taskDesc in taskDescriptions)
        {
            taskDesc.text = Translator.Translate("LaboratoryTour", task.name); // Update whiteboard text
        }
    }

        public void GameOverText(TaskList taskList)
    {
        currentTaskList = taskList;
        UpdateTaskDescriptionsWithPoints();
    }

    private void UpdateGameOverText()
    {
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
}
