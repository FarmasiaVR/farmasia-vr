using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class TestTaskDescriptionManager
{
    private TaskDescriptionManager taskDescriptionManager;
    private GameObject managerGameObject;
    private GameObject taskDesc;
    private TaskManager taskManager;
    private TaskList taskList;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Instantiate a TaskDescription in the scene to check if the updates are run correctly.
        taskDesc = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/NewTaskDescription.prefab"));

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

        // Connect the TaskDescriptionManager to TaskManager
        taskManager.onTaskStarted.AddListener(taskDescriptionManager.UpdateTaskDescriptions);
        
        // Skip a frame so that Start and Awake functions of the TaskManager are run
        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up any resources used by the tests
        Object.Destroy(taskDescriptionManager.gameObject);
        Object.Destroy(managerGameObject);
        Object.Destroy(taskDesc);
    }

    [Test]
    public void TestAssetsExist()
    {
        Assert.NotNull(taskList);
        Assert.NotNull(taskDesc);
    }

    [Test]
    public void TestTaskDescriptionCorrectOnStart()
    {
        Assert.AreEqual(taskDesc.GetComponent<TextMeshPro>().text, "Task A");
    }

    [Test]
    public void TestTaskDescriptionUpdate()
    {
        taskManager.CompleteTask("A");
        Assert.AreEqual(taskDesc.GetComponent<TextMeshPro>().text, "Task B");
    }
}
