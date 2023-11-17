using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.Events;

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
        taskManager.CompleteTask("A");
        yield return null;
        Assert.AreEqual(taskManager.currentTask.key, "B");
    }

    [UnityTest]
    public IEnumerator TestGettingSpecificTaskWorks()
    {
        Assert.AreEqual(taskManager.taskListObject.GetTask("B").key, "B");
        yield return null;
    }

    [Test]
    public void TestAllTaskCompleted()
    {
        bool eventCalled = false;

        taskManager.onAllTasksCompleted.AddListener(task => eventCalled = true);

        taskManager.CompleteTask("A");
        taskManager.CompleteTask("B");
        taskManager.CompleteTask("C");

        Assert.True(taskManager.IsTaskCompleted("A"));
        Assert.True(taskManager.IsTaskCompleted("B"));
        Assert.True(taskManager.IsTaskCompleted("C"));

        Assert.True(eventCalled);
    }

    [UnityTest]
    public IEnumerator TestCreateGeneralMistake()
    {
        UnityEvent<Mistake> onMistakeEvent = new UnityEvent<Mistake>();
        taskManager.onMistake = onMistakeEvent;
        taskManager.GenerateGeneralMistake("Mistake was made", 1);

        yield return null;
        Assert.AreEqual(1, taskManager.taskListObject.generalMistakes.Count);
        Assert.AreEqual("Mistake was made", taskManager.taskListObject.generalMistakes[0].mistakeText);
        Assert.AreEqual(1, taskManager.taskListObject.generalMistakes[0].pointsDeducted);
    }

    [UnityTest]
    public IEnumerator TestCreateTaskMistake()
    {
        UnityEvent<Mistake> onMistakeEvent = new UnityEvent<Mistake>();
        taskManager.onMistake = onMistakeEvent;
        taskManager.GenerateTaskMistake("Mistake was made", 3);
        taskManager.GenerateTaskMistake("Mistake was made again", 2);

        yield return null;
        Assert.AreEqual(2, taskManager.currentTask.ReturnMistakes().Count);
        Assert.AreEqual("Mistake was made", taskManager.currentTask.ReturnMistakes()[0].mistakeText);
        Assert.AreEqual("Mistake was made again", taskManager.currentTask.ReturnMistakes()[1].mistakeText);
        Assert.AreEqual(3, taskManager.currentTask.ReturnMistakes()[0].pointsDeducted);
        Assert.AreEqual(2, taskManager.currentTask.ReturnMistakes()[1].pointsDeducted);
    }

    [UnityTest]
    public IEnumerator TestFailedTaskDoesNotIncreasePoints()
    {
        taskManager.CompleteTask("A");
        taskManager.CompleteTask("B");
        yield return new WaitForSecondsRealtime(4);
        Assert.True(taskManager.currentTask.timed);
        Assert.True(taskManager.currentTask.failWhenOutOfTime);
        taskManager.CompleteTask("C");
        Assert.True(taskManager.IsTaskCompleted("C"));
        Assert.AreEqual(200, taskManager.taskListObject.points);
    }

    [UnityTest]
    public IEnumerator TestTimedTaskGrantsPartOfThePointsAndTaskListPointsCountingWorks()
    {
        taskManager.CompleteTask("A");
        taskManager.CompleteTask("B");
        yield return new WaitForSecondsRealtime(1);
        Assert.True(taskManager.currentTask.timed);
        taskManager.CompleteTask("C");
        Assert.True(taskManager.IsTaskCompleted("C"));
        Assert.AreEqual(100, taskManager.currentTask.awardedPoints, 5);
        Assert.AreEqual(300, taskManager.taskListObject.points, 5);
    }

    [UnityTest]
    public IEnumerator TestCompletingTasksInDifferentOrderWorks()
    {
        yield return null;
        Assert.AreEqual(taskManager.currentTask.key, "A");
        taskManager.CompleteTask("B");
        yield return null;
        Assert.True(taskManager.IsTaskCompleted("B"));
        Assert.AreEqual(taskManager.currentTask.key, "A");
        taskManager.CompleteTask("A");
        yield return null;
        Assert.True(taskManager.IsTaskCompleted("A"));
        Assert.True(taskManager.IsTaskCompleted("B"));
        Assert.AreEqual(taskManager.currentTask.key, "C");
        taskManager.CompleteTask("C");
        yield return null;
        Assert.True(taskManager.IsTaskCompleted("A"));
        Assert.True(taskManager.IsTaskCompleted("B"));
        Assert.True(taskManager.IsTaskCompleted("C"));
    }
}
