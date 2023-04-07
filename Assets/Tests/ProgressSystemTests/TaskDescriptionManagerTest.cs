using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class TestTaskDescriptionManager
{
    private TaskDescriptionManager taskDescriptionManager;
    private GameObject managerGameObject;
    private TaskManager taskManager;
    private TaskList taskList;

    [SetUp]
    public IEnumerator SetUp()
    {
        // Set up TaskDescriptionManager
        GameObject gameObject = new GameObject();
        taskDescriptionManager = gameObject.AddComponent<TaskDescriptionManager>();

        // Set up taskManager and taskList
        taskList = AssetDatabase.LoadAssetAtPath<TaskList>("Assets/Tests/ProgressSystemTests/UnitTestTasks.asset");
        managerGameObject = new GameObject();
        managerGameObject.AddComponent<TaskManager>();
        taskManager = managerGameObject.GetComponent<TaskManager>();
        taskManager.taskListObject = taskList;
        taskManager.resetOnStart = true;

        // Skip a frame so that Start and Awake functions of the TaskManager are run
        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up any resources used by the tests
        UnityEngine.Object.Destroy(taskDescriptionManager.gameObject);
        UnityEngine.Object.Destroy(managerGameObject);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestTaskDescriptionManagerWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestUpdateTaskDescriptions()
    {
        taskManager.CompleteTask("A");
        taskDescriptionManager.UpdateTaskDescriptions(taskManager.GetCurrentTask());
        yield return null;
        Assert.AreEqual("Task B", taskManager.GetCurrentTask().taskText);
    }
}
