using System;
using System.Collections;
using UnityEngine;
using FarmasiaVR.Legacy;
public class HandStateManager : MonoBehaviour {

    private HandState handState;
    private bool cleanAnimationPlayed;
    private bool shinyAnimationPlayed;

    private HandSparkleAudioFX handSparkleAudioFX;

    public HandEffectSpawner leftHandEffectSpawner;
    public HandEffectSpawner rightHandEffectSpawner;
    public Material material;

    private bool isMistake;


    public bool glovesOnAtStart;

    bool handsOutsideCabinet = false;

    public bool canAirContaminate;

    //time in seconds that it takes for hands to get contaminated again after being cleaned with cleaning bottle
    public float handWashContaminationDelay;

    //time in seconds that the hands take to get contaminated once the hands are outside the laminar cabinet
    public float handAirContaminationDelay;

    //time in seconds until the hands get air contaminated
    float timeUntilContamination;

    public void Start() {
       
        SetDirty();
        Subscribe();
        handSparkleAudioFX = FindObjectOfType<HandSparkleAudioFX>();

        if(glovesOnAtStart)
        {
            SetGlovesOn();
        }
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
        if (!glovesOnAtStart)
        {
            SetDirty();
        }
        Events.UnsubscribeFromEvent(OpenedDoor, EventType.RoomDoor);
    }

    private void TrackEquippedClothing(CallbackData data) {
        Debug.Log(data);
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

    public void SetGlovesOn()
    {
        handState = HandState.Clean;
        SetDefault();
        material.SetInt("_GlovesOn", 1);
    }

    public void SetDefault() {
        material.SetFloat("_StepEdge", 0.6f);
        material.SetInt("_Shiny", 0);
        material.SetFloat("_FresnelEffectPower", 10.0f);
        material.SetFloat("_SoapColor", 0.0f);
        material.SetInt("_GloveOn", 0);
    }

    public void SetDirty() {
        handState = HandState.Dirty;
        material.SetFloat("_StepEdge", 0.05f);
        material.SetInt("_Shiny", 0);
        material.SetFloat("_FresnelEffectPower", 10.0f);
        material.SetFloat("_SoapColor", 0f);
        material.SetInt("_GlovesOn", 0);
        cleanAnimationPlayed = false;
    }

    public void SetSoapy() {
        handState = HandState.Soapy;
        material.SetFloat("_StepEdge", 0.05f);
        StartCoroutine(Lerp(0, 0.5f, 2.0f, "_SoapColor"));
        material.SetInt("_Shiny", 0);
        cleanAnimationPlayed = false;
    }

    public void SetWet() {
        handState = HandState.Wet;
        material.SetFloat("_SoapColor", 0f);
        cleanAnimationPlayed = false;
    }

    public void SetClean() {
        handState = HandState.Clean;
        
        if (!cleanAnimationPlayed) {
            StartCoroutine(leftHandEffectSpawner.SpawnSoapBubbles());
            StartCoroutine(rightHandEffectSpawner.SpawnSoapBubbles());
            StartCoroutine(Lerp(0.05f, 0.6f, 5.0f, "_StepEdge"));
            StartCoroutine(Lerp(0.5f, 0, 4.0f, "_SoapColor"));
            cleanAnimationPlayed = true;
        }
        
    }

    public void SetCleanest() {
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

    public void airContaminateHands()
    {
        handsOutsideCabinet = true;
    }

    public void handsEnteredCabinet()
    {
        if (handState == HandState.Dirty)
        {
            Task.CreateGeneralMistake("Kaappiin viedyt kädet olivat ilman saastuttamat", 1);
        }
        handsOutsideCabinet = false;

        //this check prevents timeUntilContamination getting "reset" if it was set to higher value than handAirContaminationDelay
        //before being put inside the cabinet, for example by being cleaned with cleaning bottle
        if (timeUntilContamination < handAirContaminationDelay)
        {
            timeUntilContamination = handAirContaminationDelay;
        }
    }

    public void cleanHands()
    {
        SetGlovesOn();
        //resetting timer avoids hands getting immediately dirty after they are cleaned
        timeUntilContamination = handWashContaminationDelay;
    }

    private void FixedUpdate()
    {

        if(canAirContaminate && handsOutsideCabinet)
        {
            timeUntilContamination -= Time.deltaTime;
            if(timeUntilContamination <= 0.0f && handState != HandState.Dirty)
            {
                SetDirty();
                timeUntilContamination = 0.0f;
            }
        }
    }
}
