using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class HintManagerTests
{
    private GameObject hintBoxObject;
    private GameObject taskManagerObject;
    private TaskManager taskManager;
    private HintManager hintManager;
    private HintBoxNew hintBox;
    private TextMeshPro hintBoxText;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        //Create and init the task manager
        taskManagerObject = new GameObject();
        taskManager = taskManagerObject.AddComponent<TaskManager>();
        taskManager.taskListObject = AssetDatabase.LoadAssetAtPath<TaskList>("Assets/Tests/ProgressSystemTests/UnitTestTasks.asset");

        //Spawn a hint box in the scene
        hintBoxObject = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Systems/HintBoxNew.prefab"));
        hintBox = hintBoxObject.GetComponent<HintBoxNew>();
        hintBoxText = hintBox.hintDesc;

        hintManager = taskManagerObject.AddComponent<HintManager>();
        taskManager.onTaskStarted.AddListener(hintManager.UpdateHintDescriptions);
        taskManager.resetOnStart = true;

        yield return null;

    }

    [Test]
    public void ReferencesLoadedCorrectly()
    {
        Assert.NotNull(hintBoxObject);
        Assert.NotNull(hintBox);
        Assert.NotNull(taskManager.taskListObject);
    }

    [Test]
    public void CorrectHintShown()
    {
        Assert.AreEqual(hintBoxText.text, "Task A hint");
        taskManager.CompleteTask("A");
        Assert.AreEqual(hintBoxText.text, "Task B hint");
    }

    [Test]
    public void HintHiddenAfterTaskCompletion()
    {
        hintBox.ShowText();
        Assert.True(hintBoxText.gameObject.activeSelf);
        taskManager.CompleteTask("A");

        Assert.False(hintBoxText.gameObject.activeSelf);
    }

    [Test]
    public void HintActivationWorksCorrectly()
    {
        Assert.False(hintBoxText.gameObject.activeSelf);
        hintBox.ShowText();
        Assert.True(hintBoxText.gameObject.activeSelf);
    }

    [Test]
    public void PenalisedForOpeningHint()
    {
        hintBox.TextShownMistake();
        Assert.Less(taskManager.taskListObject.points, 0);
        Assert.Greater(taskManager.taskListObject.generalMistakes.Count, 0);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(hintBox);
        Object.Destroy(hintManager);
        Object.Destroy(hintBoxObject);
        Object.Destroy(taskManagerObject);

        yield return null;
    }

}
