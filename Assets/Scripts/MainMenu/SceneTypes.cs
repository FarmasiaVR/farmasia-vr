using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum SceneTypes {
    MainMenu,
    Tutorial,
    MedicinePreparation,
    MembraneFilteration,
    ChangingRoom,
    Restart,
    FireHazard,
    FireExtinguisherTutorial,
    FireBlanketTutorial,
    EyeShowerTutorial,
    EmergencyShowerTutorial,
    Laboratory,
    FireSafetyMenu,
    EmergencyExit,
    EmergencyExit1,
    XrPlayer,
    SafetyEquipmentTutorial,
    PlateCountMethod
}

public readonly struct GameScenes {
    private static readonly Dictionary<SceneTypes, string> sceneTypeToName = new Dictionary<SceneTypes, string> {
        { SceneTypes.Restart, "Restart" },
        { SceneTypes.MainMenu, "MainMenu" },
        { SceneTypes.MedicinePreparation, "MedicinePreparation 2.0 XR" },
        { SceneTypes.Tutorial, "ControlsTutorial" },
        { SceneTypes.MembraneFilteration, "XR MembraneFilteration 2.0" },
        { SceneTypes.ChangingRoom, "ChangingRoom" },
        { SceneTypes.FireHazard, "Laboratory"},
        { SceneTypes.FireExtinguisherTutorial, "FireExtinguisherTutorial" },
        { SceneTypes.FireBlanketTutorial, "FireBlanketTutorial" },
        { SceneTypes.EyeShowerTutorial, "EyeShowerTutorial" },
        { SceneTypes.EmergencyShowerTutorial, "EmergencyShowerTutorial" },
        { SceneTypes.FireSafetyMenu, "FireSafetyMenu" },
        { SceneTypes.EmergencyExit, "EmergencyExit" },
        { SceneTypes.EmergencyExit1, "EmergencyExit 1" },
        { SceneTypes.XrPlayer, "XR Player"},
        { SceneTypes.SafetyEquipmentTutorial, "EquipmentTutorials"},
        { SceneTypes.Laboratory, "LaboratoryLighting"},
        { SceneTypes.PlateCountMethod, "PlateCountMethod"}
    };

    private static Dictionary<string, SceneTypes> GetNameToTypeMapping() {
        Dictionary<string, SceneTypes> result = new Dictionary<string, SceneTypes>();
        try {
            result = sceneTypeToName.ToDictionary(pair => pair.Value, pair => pair.Key);
        } catch (ArgumentException ex) {
            Debug.LogWarning("There was a duplicate mapping for scene. Please remove any repeated scene names.");
        }
        return result;
    }

    private static readonly Dictionary<string, SceneTypes> sceneNameToType = GetNameToTypeMapping();

    public static string GetName(SceneTypes type) {
        if (sceneTypeToName.ContainsKey(type)) {
            return sceneTypeToName[type];
        } else {
            throw new ArgumentException($"Scene {type} doesn't have a scene name associated with it.");
        }
    }

    public static SceneTypes GetType(string name) {
        if (sceneNameToType.ContainsKey(name)) {
            return sceneNameToType[name];
        } else {
            throw new ArgumentException($"Scene {name} doesn't have a scene type associated with it.");
        }
    }

    public static SceneTypes TypeStringToType(string type) {
        if (Enum.TryParse(type, out SceneTypes result)) {
            return result;
        } else {
            throw new ArgumentException($"Scene type {type} doesn't exist.");
        }
    }
}