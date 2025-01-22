using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

#if UNITY_ANDROID
using SteamVRMock;
#else
using Valve.VR;
#endif
public class OpenXRToSteamVRTranslatorTests
{
  
    XRSimulatedController GetControllerOrCreateNew(string name, string useCase) {
        XRSimulatedController controller = (XRSimulatedController)InputSystem.GetDevice(name);
        
        //create new if we didnt find existing
        if(controller == null) 
        {
            controller = InputSystem.AddDevice<XRSimulatedController>(name);

            //add device usage tells unity what this device is used for
            //for lefthandController use "LeftHand", for righthandController use "RightHand"
            //this ensures that the mock controller looks like a real controller to the game
            InputSystem.AddDeviceUsage(controller, useCase);
        }
        
        return controller;
    }

    XRSimulatedHMD GetHMDOrCreateNew(string name) {
        XRSimulatedHMD HMD = (XRSimulatedHMD)InputSystem.GetDevice(name);

        //create new if we didnt find existing
        if (HMD == null) {
            HMD = InputSystem.AddDevice<XRSimulatedHMD>(name);

            //TODO: check correct use case for HMD, is it needed?
            //InputSystem.AddDeviceUsage(HMD, useCase);
        }

        return HMD;
    }

    void initTestControllersIfNoneExists() {
        //the names are hard coded for now, TODO: look at how we can make this more dynamic

        //added devices won't reset after playmode starts so check if we have already added simulated controllers / HMD before
        XRSimulatedController leftController = GetControllerOrCreateNew("XRSimulatedController - LeftHand", "LeftHand");
        XRSimulatedController rightController = GetControllerOrCreateNew("XRSimulatedController - RightHand", "RightHand");
        XRSimulatedHMD HMD = GetHMDOrCreateNew("XRSimulatedHMD");


        

    }
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

        initTestControllersIfNoneExists();
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

    [UnityTest]
    public IEnumerator TestSteamVRTranslatorSetsCorrectStateWhenTriggerButtonIsPressedAndReleased() {

        yield return setUpTest();
        XRSimulatedController left = GetControllerOrCreateNew("XRSimulatedController - LeftHand", "LeftHand");
        
        //press button
        InputSystem.QueueDeltaStateEvent(left.trigger, 1);
        yield return null; //wait frame
        Assert.IsTrue(VRInput.GetControl(SteamVR_Input_Sources.LeftHand, ControlType.TriggerClick));
        yield return null;

        //release button
        InputSystem.QueueDeltaStateEvent(left.trigger, 0);
        yield return null; //wait frame
        Assert.IsFalse(VRInput.GetControl(SteamVR_Input_Sources.LeftHand, ControlType.TriggerClick));
    }

   
    [UnityTest]
    public IEnumerator SteamVRTranslatorSetsCorrectStateWhenFillMedicineButtonIsPressedAndReleasedPosX()
    {

        yield return setUpTest();
        

        //left hand controller
        XRSimulatedController left = GetControllerOrCreateNew("XRSimulatedController - LeftHand", "LeftHand");
        //press button
        InputSystem.QueueDeltaStateEvent(left.primary2DAxis, new Vector2(1.0f, 0.0f));
        yield return null; //wait frame
        Assert.IsTrue(VRInput.GetControl(SteamVR_Input_Sources.LeftHand, ControlType.DPadWest));
        yield return null;

        //release button
        InputSystem.QueueDeltaStateEvent(left.primary2DAxis, new Vector2(0.0f, 0.0f));
        yield return null; //wait frame
        Assert.IsFalse(VRInput.GetControl(SteamVR_Input_Sources.LeftHand, ControlType.DPadWest));


        //right hand controller
        XRSimulatedController right = GetControllerOrCreateNew("XRSimulatedController - RightHand", "RightHand");
        //press button
        InputSystem.QueueDeltaStateEvent(right.primary2DAxis, new Vector2(1.0f, 0.0f));
        yield return null; //wait frame
        Assert.IsTrue(VRInput.GetControl(SteamVR_Input_Sources.RightHand, ControlType.DPadWest));
        yield return null;

        //release button
        InputSystem.QueueDeltaStateEvent(right.primary2DAxis, new Vector2(0.0f, 0.0f));
        yield return null; //wait frame
        Assert.IsFalse(VRInput.GetControl(SteamVR_Input_Sources.RightHand, ControlType.DPadWest));
    }

    [UnityTest]
    public IEnumerator SteamVRTranslatorSetsCorrectStateWhenFillMedicineButtonIsPressedAndReleasedNegX()
    {

        yield return setUpTest();


        //left hand controller
        XRSimulatedController left = GetControllerOrCreateNew("XRSimulatedController - LeftHand", "LeftHand");
        //press button
        InputSystem.QueueDeltaStateEvent(left.primary2DAxis, new Vector2(-1.0f, 0.0f));
        yield return null; //wait frame
        Assert.IsTrue(VRInput.GetControl(SteamVR_Input_Sources.LeftHand, ControlType.DPadEast));
        yield return null;

        //release button
        InputSystem.QueueDeltaStateEvent(left.primary2DAxis, new Vector2(0.0f, 0.0f));
        yield return null; //wait frame
        Assert.IsFalse(VRInput.GetControl(SteamVR_Input_Sources.LeftHand, ControlType.DPadEast));


        //right hand controller
        XRSimulatedController right = GetControllerOrCreateNew("XRSimulatedController - RightHand", "RightHand");
        //press button
        InputSystem.QueueDeltaStateEvent(right.primary2DAxis, new Vector2(-1.0f, 0.0f));
        yield return null; //wait frame
        Assert.IsTrue(VRInput.GetControl(SteamVR_Input_Sources.RightHand, ControlType.DPadEast));
        yield return null;

        //release button
        InputSystem.QueueDeltaStateEvent(right.primary2DAxis, new Vector2(0.0f, 0.0f));
        yield return null; //wait frame
        Assert.IsFalse(VRInput.GetControl(SteamVR_Input_Sources.RightHand, ControlType.DPadEast));
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
