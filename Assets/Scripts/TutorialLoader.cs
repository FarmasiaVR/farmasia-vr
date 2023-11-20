using UnityEngine;
using System;
using System.Collections.Generic;

public enum TutorialType {
    FireExtinguisher, FireBlanket, EmergencyShower, EyeShower
}

public class TutorialLoader : MonoBehaviour {
    private Dictionary<TutorialType, GameObject> tutorialTypeToParentObject = new Dictionary<TutorialType, GameObject>();
    public static TutorialType? tutorialToLoad = null;
    [SerializeField] private GameObject fireExtinguisherTutorialParent, fireBlanketTutorialParent, emergencyShowerTutorialParent, eyeShowerTutorialParent;

    private void Start() {
        // Initialize global data if null and otherwise load the tutorial that is set in global data
        if (tutorialToLoad != null) {
            tutorialTypeToParentObject[TutorialType.FireExtinguisher] = fireExtinguisherTutorialParent;
            tutorialTypeToParentObject[TutorialType.FireBlanket] = fireBlanketTutorialParent;
            tutorialTypeToParentObject[TutorialType.EmergencyShower] = emergencyShowerTutorialParent;
            tutorialTypeToParentObject[TutorialType.EyeShower] = eyeShowerTutorialParent;
            LoadSetTutorial();
        }
    }

    public void SetTutorialToLoad(string tutorialName) {
        if (Enum.TryParse(tutorialName, out TutorialType tutorialType)) {
            tutorialToLoad = tutorialType;
        } else {
            throw new ArgumentException($"Tutorial type {tutorialName} doesn't exist.");
        }
    }

    private void DisableObjectAndChildren(GameObject obj) {
        // Disable parent GameObject
        obj.SetActive(false);

        // Disable its children recursively
        foreach (Transform child in obj.transform)
            DisableObjectAndChildren(child.gameObject);
    }

    public void LoadSetTutorial() {
        if (tutorialToLoad is TutorialType type) {
            // Disable tutorials other than the target tutorial
            foreach (TutorialType otherType in tutorialTypeToParentObject.Keys) {
                if (type != otherType) {
                    DisableObjectAndChildren(tutorialTypeToParentObject[otherType]);
                }
            }
            // Teleport player to target tutorial's start point and reset tutorial type
            gameObject.GetComponent<PlayerTeleporter>().TeleportPlayer(type.ToString() + "Tutorial");
            tutorialToLoad = null;
        } else {
            throw new Exception($"You need to set the tutorial type to load before trying to load a specific tutorial");
        }
    }
}