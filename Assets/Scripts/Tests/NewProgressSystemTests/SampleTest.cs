using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
public class SampleTest
{

    [UnityTest]
    public IEnumerator TaskManagerExistsInTestScene()
    {
        LogAssert.ignoreFailingMessages = true;
        yield return new EnterPlayMode();
        SceneManager.LoadScene("TaskManagerExample", LoadSceneMode.Single);
        yield return null; // <--- skip frame
        //Warning: this supposes that there is exaclty one GameManager object in that scene that we loaded
        GameObject testObject = GameObject.Find("GameManager");
        TaskManager manager = testObject.GetComponent<TaskManager>();
        Assert.IsNotNull(manager);

        

        Debug.Log("hello world from test scene!");

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
