using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class TaskManagerTest
{
    [SerializeField]

    private TaskList taskList;
    private GameObject managerGameObject;
    private TaskManager taskManager;


    [UnitySetUp]
    public IEnumerator SetUp()
    {
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
        Object.Destroy(managerGameObject);
    }

    [Test]
    public void TestReferencesCorrect()
    {
        Assert.NotNull(taskList);
        Assert.NotNull(taskManager.taskListObject);

    }

    [Test]
    public void TestMarkingAsDone()
    {
        Assert.False(taskManager.IsTaskCompleted("A"));
        taskManager.CompleteTask("A");
        Assert.True(taskManager.IsTaskCompleted("A"));
    }

    [UnityTest]
    public IEnumerator TestCompleteEventInvoked()
    {
        bool eventIsCalled = false;

        taskManager.onTaskCompleted.AddListener(task => { eventIsCalled = true; });
        taskManager.CompleteTask("A");
        // Skip a frame so that the onTaskComplete event is properly called.
        yield return null;

        Assert.True(eventIsCalled);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestManagerTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

