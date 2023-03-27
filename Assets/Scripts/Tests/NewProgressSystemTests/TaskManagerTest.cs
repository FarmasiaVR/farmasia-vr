using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
public class TaskManagerTest
{
    [SetUp]
    public void SetUp()
    {
        LogAssert.ignoreFailingMessages = true;
        SceneManager.LoadScene("TaskManagerExample", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator TaskManagerExistsInTestScene()
    {
        yield return new EnterPlayMode();
        yield return null; // <--- skip frame
        
        //Warning: this supposes that there is exaclty one GameManager object in that scene that we loaded
        GameObject testObject = GameObject.Find("GameManager");
        TaskManager manager = testObject.GetComponent<TaskManager>();
        Assert.IsNotNull(manager);

        yield return null;
    }

    [UnityTest]
    public IEnumerator CreatingTaskListWorks()
    {

    }
}
