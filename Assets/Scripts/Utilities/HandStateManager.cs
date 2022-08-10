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

    private bool isMistake;

    public void Start() {
        SetDirty();
        Subscribe();
        handSparkleAudioFX = FindObjectOfType<HandSparkleAudioFX>();
    }

    public void Subscribe() {
        Events.SubscribeToEvent(OpenedDoor, EventType.RoomDoor);
        Events.SubscribeToEvent(TrackEquippedClothing, EventType.ProtectiveClothingEquipped);
    }

    public void WashingHands(CallbackData data) {
        TaskType currentTask = G.Instance.Progress.CurrentPackage.CurrentTask.TaskType;
        if (currentTask == TaskType.WashHandsInChangingRoom || currentTask == TaskType.WashHandsInPreperationRoom) {
            var liquid = (data.DataObject as HandWashingLiquid);

            if (liquid.type.Equals("Soap")) {
                if (handState == HandState.Clean) {
                    SetIsMistake(true);
                    SetSoapy();
                } else if (handState != HandState.Clean) {
                    SetIsMistake(false);
                    SetSoapy();
                }
            } else if (liquid.type.Equals("Water")) {
                if (handState == HandState.Dirty) {
                    SetIsMistake(false);
                    SetWet();
                } else if (handState == HandState.Wet) {
                    SetIsMistake(false);
                    SetWet();
                } else if (handState == HandState.Soapy) {
                    SetIsMistake(false);
                    SetClean();
                } else if (handState == HandState.Clean) {
                    SetIsMistake(false);
                    SetClean();
                }
            } else if (liquid.type.Equals("HandSanitizer")) {
                if (handState != HandState.Clean) {
                    SetIsMistake(true);
                    SetDirty();
                } else if (handState == HandState.Clean) {
                    SetIsMistake(false);
                    SetCleanest();
                }
            }
        }
    }

    public bool GetIsMistake() {
        return isMistake;
    }

    private void SetIsMistake(bool m) {
        isMistake = m;
    }

    private void OpenedDoor(CallbackData data) {
        cleanAnimationPlayed = false;
        shinyAnimationPlayed = false;
        SetDirty();
        Events.UnsubscribeFromEvent(OpenedDoor, EventType.RoomDoor);
    }

    private void TrackEquippedClothing(CallbackData data) {
        var clothing = (data.DataObject as ProtectiveClothing);
        if (clothing.type == ClothingType.ProtectiveGloves && handState == HandState.Cleanest) {
            SetDefault();
            material.SetInt("_GlovesOn", 1);
            Events.UnsubscribeFromEvent(TrackEquippedClothing, EventType.ProtectiveClothingEquipped);
        }
    }

    public HandState GetHandState() {
        return handState;
    }

    private void SetDefault() {
        material.SetFloat("_StepEdge", 0.6f);
        material.SetInt("_Shiny", 0);
        material.SetFloat("_FresnelEffectPower", 10.0f);
        material.SetFloat("_SoapColor", 0.0f);
        material.SetInt("_GloveOn", 0);
    }

    private void SetDirty() {
        handState = HandState.Dirty;
        material.SetFloat("_StepEdge", 0.05f);
        material.SetInt("_Shiny", 0);
        material.SetFloat("_FresnelEffectPower", 10.0f);
        material.SetFloat("_SoapColor", 0f);
        material.SetInt("_GlovesOn", 0);
        cleanAnimationPlayed = false;
    }

    private void SetSoapy() {
        handState = HandState.Soapy;
        material.SetFloat("_StepEdge", 0.05f);
        StartCoroutine(Lerp(0, 0.5f, 2.0f, "_SoapColor"));
        material.SetInt("_Shiny", 0);
        cleanAnimationPlayed = false;
    }

    private void SetWet() {
        handState = HandState.Wet;
        material.SetFloat("_SoapColor", 0f);
        cleanAnimationPlayed = false;
    }

    private void SetClean() {
        handState = HandState.Clean;
        if (!cleanAnimationPlayed) {
            StartCoroutine(leftHandEffectSpawner.SpawnSoapBubbles());
            StartCoroutine(rightHandEffectSpawner.SpawnSoapBubbles());
            StartCoroutine(Lerp(0.05f, 0.6f, 5.0f, "_StepEdge"));
            StartCoroutine(Lerp(0.5f, 0, 4.0f, "_SoapColor"));
            cleanAnimationPlayed = true;
        }
    }

    private void SetCleanest() {
        handState = HandState.Cleanest;
        if (!shinyAnimationPlayed) {
            material.SetFloat("_SoapColor", 0f);
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
