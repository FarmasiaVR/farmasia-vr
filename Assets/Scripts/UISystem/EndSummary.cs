using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class EndSummary : MonoBehaviour {

    #region fields
    private const string TAG = "EndSummary";

    private Camera cam;
    private DragAcceptable close;
    #endregion

    private void Start() {
        cam = transform.GetComponentInChildren<Camera>();
        cam.enabled = false;
        SetChildStatuses(false);

        close = transform.Find("CloseButton").GetComponent<DragAcceptable>();
        close.OnAccept = CloseGame;
    }

    private void SetChildStatuses(bool status) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(status);
        }
    }

    private void EnableSummary(string summary) {
        SetChildStatuses(true);

        TextMeshPro text = transform.Find("Text").GetComponent<TextMeshPro>();

        if (text == null) {
            Logger.Error("Text mesh pro was null in end summary");
            return;
        }

        text.text = summary;
    }

    private async void CloseGame() {

        close.SafeDestroy();

        await Task.Delay(2000);

        try {
            SnapScreenshot();
        } catch (System.Exception) {
        }

        Application.Quit();
    }

    private void SnapScreenshot() {

        HandMeshToggler[] handMeshes = GameObject.FindObjectsOfType<HandMeshToggler>();
        bool[] statuses = new bool[2] { handMeshes[0].Status, handMeshes[1].Status };

        handMeshes[0].Show(false);
        handMeshes[1].Show(false);

        cam.gameObject.SetActive(true);
        cam.enabled = true;

        string filePath = GetPath();

        int width = 1920;
        int height = 1080;

        RenderTexture tex = new RenderTexture(width, height, 24);

        cam.targetTexture = tex;
        Texture2D scr = new Texture2D(width, height);

        cam.Render();

        RenderTexture.active = tex;
        scr.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        byte[] bytes = scr.EncodeToJPG(100);
        System.IO.File.WriteAllBytes(filePath, bytes);

        cam.enabled = false;

        handMeshes[0].Show(statuses[0]);
        handMeshes[1].Show(statuses[1]);
    }
    private static string GetPath() {
        if (Application.isEditor) {
            return Application.dataPath + "/score_screenshot.jpg";
        } else {
            return Application.dataPath + "/../../score_screenshot.jpg";
        }
    }

    public static void EnableEndSummary(string summary) {
        GameObject.FindGameObjectWithTag(TAG).GetComponent<EndSummary>().EnableSummary(summary);
    }
}