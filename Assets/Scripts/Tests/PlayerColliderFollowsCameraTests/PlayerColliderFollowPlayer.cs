using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerColliderFollowPlayer
{   
    //I know that this is manual copy paste
    //TODO: make this a function that can be used easily from any test!!
    private IEnumerator setUpTest(int sceneIndex, string sceneName)
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return new EnterPlayMode();
        //scene 6 is the scene where the XR rig for steamVR exists
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        yield return null;
        yield return null;
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        yield return null;
        Scene current = SceneManager.GetActiveScene();
        Debug.Log("Scene in test is at index: " + current.buildIndex);
        yield return null;
        yield return null;
    }



    [UnityTest]
    public IEnumerator colliderPositionUpdatesWhenPlayerPosUpdates()
    {
        //TODO: maybe have our own scene with only player?
        yield return setUpTest(4, "ChangingRoom");

        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        Assert.That(cameras.Length == 1);
        GameObject camera = cameras[0];

        
        camera.transform.position = new Vector3(2.0f, 1.0f, 3.0f);
        yield return null;
        GameObject colliderObj = GameObject.FindGameObjectWithTag("PlayerCollider");
        //Assert.AreEqual(colliderObj.transform.position.x, camera.transform.position.x);
        //Assert.AreEqual(colliderObj.transform.position.z, camera.transform.position.z);
        yield return null;
    }


    
}
