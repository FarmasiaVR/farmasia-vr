using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour {

    [Serializable]
    public struct ObjectHintPair {
        public ObjectType type;
        public string hint;
    }

    [Serializable]
    public struct TutorialVideoPair {
        public string name;
        public Video video;
    }

    [Serializable]
    public struct Video {
        public VideoClip clip;
        public string title;
    }

    private Dictionary<ObjectType, string> hints;
    private Dictionary<string, Video> videos;
    private GameObject currentTutorial;

    public GameObject returnButton;
    public GameObject tutorials;
    public Transform videoHintSpawn;
    public ObjectHintPair[] tutorialHints;
    public TutorialVideoPair[] tutorialVideos;

    public void Start() {
        Events.SubscribeToEvent(OnGrab, this, EventType.GrabObject);
        hints = new Dictionary<ObjectType, string>();
        foreach (var pair in tutorialHints) {
            hints[pair.type] = pair.hint;
        }
        videos = new Dictionary<string, Video>();
        foreach (var pair in tutorialVideos) {
            videos[pair.name] = pair.video;
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
    }

    public void SpawnTutorial(GameObject tutorial) {
        // Instantiate(tutorial);
        tutorial.SetActive(true);
        currentTutorial = tutorial;
        HideTutorialMenu();
        if (videos.ContainsKey(tutorial.name)) {
            CreateVideoHint(videos[tutorial.name]);
        }
    }

    public void ShowTutorialMenu() {
        returnButton.SetActive(false);
        tutorials.SetActive(true);
        // temp solution
        Destroy(GameObject.Find("VideoHint(Clone)"));
        Destroy(GameObject.Find("PlayButton"));
        Destroy(GameObject.Find("FloatingHint(Clone)"));
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "CloseButton");
        foreach (var o in objects) {
            Destroy(o);
        }
        // Destroy(currentTutorial);
        currentTutorial.SetActive(false);
    }

    private void HideTutorialMenu() {
        returnButton.SetActive(true);
        tutorials.SetActive(false);
    }

    private void CreateHint(string hint) {
        HintBox.CreateHint(hint, true);
    }

    private void CreateVideoHint(Video video) {
        VideoHint.CreateVideoHint(video.clip, video.title, videoHintSpawn.position);
    }

    private string GetHintString(ObjectType type, string hint) {
        return TypeToString(type) + "\n\n" + hint;
    }

    private string TypeToString(ObjectType type) {
        switch (type) {
            case ObjectType.Luerlock:
                return "Luer-lock";
            case ObjectType.Needle:
                return "Neula";
            case ObjectType.Syringe:
                return "Ruisku";
            case ObjectType.Pipette:
                return "Pipetti";
            case ObjectType.AutomaticPipette:
                return "Pipetintäyttäjä";
            case ObjectType.Medicine:
                return "Lääkepullo";
            case ObjectType.SterileBag:
                return "Sterilointipussi";
            case ObjectType.SyringeCap:
                return "Ruiskun korkki";
            case ObjectType.DisinfectingCloth:
                return "Steriili puhdistusliina";
            case ObjectType.DisinfectantBottle:
                return "Etanolipullo";
            case ObjectType.AgarPlate:
                return "Agarmalja";
            case ObjectType.Pen:
                return "Kynä";
            case ObjectType.PumpFilterBase:
                return "Kalvosuodatin";
            case ObjectType.Pump:
                return "Pumppu";
            case ObjectType.Scalpel:
                return "Skalpelli";
            case ObjectType.Tweezers:
                return "Pinsetit";
            default:
                return type.ToString();
        }
    }
}
