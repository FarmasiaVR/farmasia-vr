using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRVibrationManager : MonoBehaviour
{
    public static VRVibrationManager Instance { get; private set; }
    public SteamVR_Action_Vibration ControllerVibration;
    public SteamVR_Action_Single Amplitude;
    public SteamVR_Action_Single Frequency;
    public SteamVR_Action_Single Strength;

    [Range(0, 1)]
    public float TestingAmplitude;

    [Range(0, 320)]
    public float TestingFrequency;

    [Range(0, 1)]
    public float TestingStrength;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void Update() {
        Pulse(0.1f, (Frequency.axis * Strength.axis), (Amplitude.axis * Strength.axis), SteamVR_Input_Sources.LeftHand);
    }

    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source) {
        ControllerVibration.Execute(0, duration, frequency, amplitude, source);
    }

    public void TriggerVibration() {
        ControllerVibration.Execute(0, 0.1f, (TestingFrequency * TestingStrength), (TestingAmplitude * TestingStrength), SteamVR_Input_Sources.LeftHand);
    }
}
