using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialScene : SceneScript {

    [Serializable]
    public struct TypeMessagePair {
        public ObjectType Type;
        public string Message;
    }

    [Serializable]
    public struct TypeVideoPair {
        public ObjectType Type;
        public Video Video;
    }

    [Serializable]
    public struct Video {
        public VideoClip Clip;
        public string Title;
    }

    #region fields
    [SerializeField]
    private GameObject walls;

    [SerializeField]
    private Transform videoHintSpawn;

    [SerializeField]
    private Video startingVideo;

    [SerializeField]
    private TypeMessagePair[] tutorialHints;

    [SerializeField]
    private TypeVideoPair[] tutorialVideoHints;

    private Dictionary<ObjectType, string> hints;
    private Dictionary<ObjectType, Video> videos;
    #endregion

    protected override void Start() {

        base.Start();

        walls.SetActive(true);

        Events.SubscribeToEvent(OnGrab, this, EventType.GrabObject);

        hints = new Dictionary<ObjectType, string>();
        foreach (var pair in tutorialHints) {
            if (!hints.ContainsKey(pair.Type)) {
                hints.Add(pair.Type, pair.Message);
            } else {
                hints[pair.Type] = pair.Message;
            }
        }

        videos = new Dictionary<ObjectType, Video>();
        foreach (var pair in tutorialVideoHints) {
            if (!hints.ContainsKey(pair.Type)) {
                videos.Add(pair.Type, pair.Video);
            } else {
                videos[pair.Type] = pair.Video;
            }
        }

        CreateVideoHint(startingVideo);
    }

    private void Update() {
        // TODO: Can this check be avoided?
        if (VideoHint.CurrentVideo == null) {
            CreateVideoHint(startingVideo);
        }
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
        if (videos.ContainsKey(item.ObjectType)) {
            CreateVideoHint(videos[item.ObjectType]);
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
            case ObjectType.Bottle:
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
        HintBox.CreateHint(hint, true);
    }
    private void CreateVideoHint(Video video) {
        VideoHint.CreateVideoHint(video.Clip, video.Title, videoHintSpawn.position);
    }
}

