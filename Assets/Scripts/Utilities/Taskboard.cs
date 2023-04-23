using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FarmasiaVR.New;

public class Taskboard : MonoBehaviour
{
    public TMP_FontAsset textFont;
    public float textStartingDistance;
    public float textSpacing;
    public float fontSize;
    public Color completedColor = Color.green;

    private Dictionary<string, TextMeshProUGUI> taskTexts = new Dictionary<string, TextMeshProUGUI>();
    private float textSpawnHeight;

    private void Awake()
    {
        textSpawnHeight = textStartingDistance;
    }
    /// <summary>
    /// This is used to initialise a taskboard to show the tasks that the player needs to complete.
    /// This creates a separate text object for every task and stores it in a dictionary so that it can be easily referred to and modified.
    /// </summary>
    /// <param name="taskList">The tasks that should be displayed</param>
    public void InitTaskboard(TaskList taskList)
    {
        foreach (Task task in taskList.tasks)
        {
            // Spawn a new text object that will show the task's information
            GameObject spawnedTextObject = new GameObject(task.key + "_text");
            spawnedTextObject.transform.parent = transform;
            spawnedTextObject.transform.localPosition = Vector3.zero;
            spawnedTextObject.transform.rotation = spawnedTextObject.transform.parent.rotation;

            TextMeshProUGUI spawnedTaskText = spawnedTextObject.AddComponent<TextMeshProUGUI>();
            spawnedTaskText.text = task.taskText;
            spawnedTaskText.font = textFont;
            spawnedTaskText.alignment = TextAlignmentOptions.Center;

            // Set the anchor to the top
            spawnedTaskText.rectTransform.anchorMin = new Vector2(0.5f, 1f);
            spawnedTaskText.rectTransform.anchorMax = new Vector2(0.5f, 1f);
            spawnedTaskText.rectTransform.anchoredPosition = new Vector3(0, textSpawnHeight, 0);
            spawnedTaskText.rectTransform.localScale = Vector3.one;
            spawnedTaskText.fontSize = fontSize;
            textSpawnHeight -= textSpacing;

            taskTexts.Add(task.key, spawnedTaskText);

            if (task.completed)
            {
                MarkTaskAsCompleted(task.key);
            }
        }
    }

    /// <summary>
    /// Marks the task with the given key as marked by painting the text green.
    /// </summary>
    /// <param name="taskKey">The key of the task that was completed.</param>
    public void MarkTaskAsCompleted(string  taskKey)
    {
        taskTexts[taskKey].color = completedColor;
    }
}
