using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class OpenXRToSteamVRTranslatorTests
{
    

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    
    private IEnumerator setUpTest()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return new EnterPlayMode();
        //scene 6 is the scene where the XR rig for steamVR exists
        SceneManager.LoadScene(6, LoadSceneMode.Single);
        yield return null;
        yield return null;
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("OpenXrToSteamVrInputTests"));
        yield return null;
        Scene current = SceneManager.GetActiveScene();

       


        Debug.Log("Scene in test is at index: " + current.buildIndex);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestSceneLoadedCorrectly()
    {

        yield return setUpTest();

        //for testing MonoBehaviours:
        //https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/reference-tests-monobehaviour.html 
        yield return new MonoBehaviourTest<MyMonoBehaviourTest>();


        yield return null;
    }

    public class MyMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
    {
        private int frameCount;
        public bool IsTestFinished
        {
            get { return frameCount > 10; }
        }

        void Start()
        {
            GameObject testedObj = GameObject.FindGameObjectWithTag("ObjectToBeTested");
            Assert.That(testedObj != null);            
        }


        void Update()
        {
            frameCount++;
        }
    }

}
