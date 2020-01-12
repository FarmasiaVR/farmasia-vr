using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialScene : MonoBehaviour {

    #region fields
    [SerializeField]
    private GameObject walls;

    [SerializeField]
    private Transform videoHintSpawn;

    [SerializeField]
    private TypeMessagePair[] tutorialHints;

    private Dictionary<ObjectType, string> hints;
    #endregion

    private void Start() {

        walls.SetActive(true);

        Events.SubscribeToEvent(OnGrab, EventType.GrabObject);

        hints = new Dictionary<ObjectType, string>();
        foreach (var pair in tutorialHints) {
            if (!hints.ContainsKey(pair.Type)) {
                hints.Add(pair.Type, pair.Message);
            } else {
                hints[pair.Type] = pair.Message;
            }
        }

        CreateVideoHint("Video title");
    }

    private void OnGrab(CallbackData data) {

        Interactable interactable = ((Hand)data.DataObject).Connector.GrabbedInteractable;

        if (interactable == null) {
            return;
        }

        GeneralItem item = interactable as GeneralItem;

        if (item == null) {
            return;
        }

        if (hints.ContainsKey(item.ObjectType)) {
            CreateHint(GetHintString(item.ObjectType, hints[item.ObjectType]));
        }
    }

    private string GetHintString(ObjectType type, string message) {
        return TypeToString(type)+ "\n\n" + message;
    }
    private string TypeToString(ObjectType type) {
        switch (type) {
            case ObjectType.Syringe:
                return "Ruisku";
            case ObjectType.Needle:
                return "Neula";
            case ObjectType.Medicine:
                return "Lääkepullo";
            case ObjectType.DisinfectingCloth:
                return "Steriili puhdistusliina";
            case ObjectType.SterileBag:
                return "Steriili pussi";
            case ObjectType.SyringeCap:
                return "Ruiskun korkki";
            default:
                return type.ToString();
        }
    }
    
    private void CreateHint(string hint) {
        HintBox.CreateHint(hint);
    }
    private void CreateVideoHint(string title) {
        VideoClip clip = Resources.Load<VideoClip>("Testing/video");

        VideoHint.CreateVideoHint(clip, title, videoHintSpawn.position);
    }
}

[Serializable]
public struct TypeMessagePair {
    public ObjectType Type;
    public string Message;
}