using System;
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
    EmergencyExitTutorial,
    EmergencyExit,
    EmergencyExit1
}

public readonly struct GameScenes {
    private static readonly Dictionary<SceneTypes, string> sceneTypeToName = new Dictionary<SceneTypes, string> {
        { SceneTypes.Restart, "Restart" },
        { SceneTypes.MainMenu, "MainMenu" },
        { SceneTypes.MedicinePreparation, "MedicinePreparation 2.0 XR" },
        { SceneTypes.Tutorial, "ControlsTutorial" },
        { SceneTypes.MembraneFilteration, "XR MembraneFilteration 2.0" },
        { SceneTypes.ChangingRoom, "ChangingRoom" },
        { SceneTypes.Laboratory, "Laboratory" },
        { SceneTypes.FireExtinguisherTutorial, "FireExtinguisherTutorial" },
        { SceneTypes.FireBlanketTutorial, "FireBlanketTutorial" },
        { SceneTypes.EyeShowerTutorial, "EyeShowerTutorial" },
        { SceneTypes.EmergencyShowerTutorial, "EmergencyShowerTutorial" },
        { SceneTypes.EmergencyExitTutorial, "EmergencyExitTutorial" },
        { SceneTypes.EmergencyExit, "EmergencyExit" },
        { SceneTypes.EmergencyExit1, "EmergencyExit 1" }
    };

    private static readonly Dictionary<string, SceneTypes> sceneNameToType = //Reverse mapping of the previous Dictionary
        sceneTypeToName.ToDictionary(pair => pair.Value, pair => pair.Key);

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
}