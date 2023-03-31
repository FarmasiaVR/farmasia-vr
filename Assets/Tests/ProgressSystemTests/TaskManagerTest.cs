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

    [UnityTest]
    public IEnumerator TestGettingCurrentTaskWorks()
    {
        Assert.AreEqual(taskManager.GetCurrentTask().key, "A");
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestGettingSpecificTaskWorks()
    {
        Assert.AreEqual(taskManager.taskListObject.GetTask("B").key, "B");
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestAllTaskCompleted()
    {
        taskManager.CompleteTask("A");
        taskManager.CompleteTask("B");
        taskManager.CompleteTask("C");
        Assert.True(taskManager.IsTaskCompleted("A"));
        Assert.True(taskManager.IsTaskCompleted("B"));
        Assert.True(taskManager.IsTaskCompleted("C"));
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestTimedTaskNotComplitedWhenTimeRunsOut()
    {
        taskManager.taskListObject.GetTask("C");
        yield return new WaitForSeconds(4);
        taskManager.CompleteTask("C"); //doesnt work when this is added
        Assert.False(taskManager.IsTaskCompleted("C"));
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestMistakeReducePoints()
    {
        taskManager.taskListObject.GetTask("B");
        Debug.Log(taskManager.taskListObject.GetPoints());
        taskManager.GenerateTaskMistake("Test Mistake Text", 1);
        Assert.AreEqual(taskManager.taskListObject.GetPoints(), 0);
        yield return null;
    }
}

