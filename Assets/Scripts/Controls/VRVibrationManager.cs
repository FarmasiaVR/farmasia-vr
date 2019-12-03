using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRVibrationManager : MonoBehaviour {

    #region Fields
    private static GameObject vibrationManager;
    public SteamVR_Action_Vibration hapticAction;
    #endregion

    [Range(0, 1)]
    public float TestingAmplitude;

    [Range(0, 320)]
    public float TestingFrequency;

    [Range(0, 1)]
    public float TestingStrength;

    void Awake() {
        vibrationManager = GameObject.FindWithTag("VibrationManager");
        if (vibrationManager == null) {
            Logger.Error("Did not find a gameObject tagged with VibrationManager, vibration feedback will not work");
        }
    }

    public static void Vibrate() {
        vibrationManager = GameObject.FindWithTag("VibrationManager");
        if (vibrationManager == null) {
            Logger.Error("Missing gameobject tagged with VibrationManager");
            return;
        }
        vibrationManager.GetComponent<VRVibrationManager>().TriggerVibration();
    }

    #region Public methods
    public void TriggerVibration() { 
        hapticAction.Execute(0, 0.1f, (TestingFrequency * TestingStrength), (TestingAmplitude * TestingStrength), SteamVR_Input_Sources.LeftHand);
    }
    #endregion
}
