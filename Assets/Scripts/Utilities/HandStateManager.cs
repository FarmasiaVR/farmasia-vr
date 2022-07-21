using System.Collections;
using UnityEngine;

public class HandStateManager : MonoBehaviour {

    private HandState handState;
    private bool cleanAnimationPlayed;
    private bool shinyAnimationPlayed;

    private HandSparkleAudioFX handSparkleAudioFX;

    public HandEffectSpawner leftHandEffectSpawner;
    public HandEffectSpawner rightHandEffectSpawner;
    public Material material;

    public void Start() {
        SetDirty();
        Subscribe();
        handSparkleAudioFX = FindObjectOfType<HandSparkleAudioFX>();
    }

    public void Subscribe() {
        Events.SubscribeToEvent(OpenedDoor, EventType.RoomDoor);
    }

    public void WashingHands(CallbackData data) {
        TaskType currentTask = G.Instance.Progress.CurrentPackage.CurrentTask.TaskType;
        if (currentTask == TaskType.WashHandsInChangingRoom || currentTask == TaskType.WashHandsInPreperationRoom) {
            var liquid = (data.DataObject as HandWashingLiquid);
            if (liquid.type.Equals("Soap") && handState != HandState.Cleanest) SetSoapy(); // OK
            else if (liquid.type.Equals("Water") && handState != HandState.Soapy) SetWet(); // MISTAKE
            else if (liquid.type.Equals("Water") && handState == HandState.Soapy) SetClean(); // OK
            else if (liquid.type.Equals("HandSanitizer") && handState != HandState.Clean) SetDirty(); // MISTAKE
            else if (liquid.type.Equals("HandSanitizer") && handState == HandState.Clean) SetCleanest(); // OK
        }
    }

    private void OpenedDoor(CallbackData data) {
        cleanAnimationPlayed = false;
        shinyAnimationPlayed = false;
        SetDirty();
        Events.UnsubscribeFromEvent(OpenedDoor, EventType.RoomDoor);
    }

    public HandState GetHandState() {
        return handState;
    }

    private void SetDirty() {
        handState = HandState.Dirty;
        material.SetFloat("_StepEdge", 0.05f);
        material.SetInt("_Shiny", 0);
        material.SetFloat("_FresnelEffectPower", 10.0f);
    }

    private void SetSoapy() {
        handState = HandState.Soapy;
    }

    private void SetWet() {
        handState = HandState.Wet;
    }

    private void SetClean() {
        handState = HandState.Clean;
        if (!cleanAnimationPlayed) {
            StartCoroutine(leftHandEffectSpawner.SpawnSoapBubbles());
            StartCoroutine(rightHandEffectSpawner.SpawnSoapBubbles());
            StartCoroutine(Lerp(0.05f, 0.6f, 6.0f, "_StepEdge"));
            cleanAnimationPlayed = true;
        }
    }

    private void SetCleanest() {
        handState = HandState.Cleanest;
        if (!shinyAnimationPlayed) {
            StartCoroutine(leftHandEffectSpawner.SpawnLensFlares());
            StartCoroutine(rightHandEffectSpawner.SpawnLensFlares());
            material.SetInt("_Shiny", 1);
            StartCoroutine(Lerp(10.0f, 2.0f, 1.0f, "_FresnelEffectPower"));
            shinyAnimationPlayed = true;
        }

        handSparkleAudioFX.PlayAudioFX();
    }

    private IEnumerator Lerp(float a, float b, float duration, string property) {
        float elapsed = 0.0f;
        while (elapsed < duration) {
            material.SetFloat(property, Mathf.Lerp(a, b, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
