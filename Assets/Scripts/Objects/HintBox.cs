using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HintBox : DragAcceptable {

    #region Fields
    #region Settings
    private static bool initialized = false;
    private static float defaultDistance = 0.5f;
    private static Vector3[] positions;
    private static float maxDistance = 2f;
    private static float hintMaxAngleDiff = 75;

    private static float viewLimitX = 0.8f;
    private static float viewLimitY = 0.6f;

    private static HintBox currentHint;
    private static HintText currentHintText;
    #endregion

    private static GameObject hintPrefab;
    private static GameObject hintTextPrefab;

    private Transform questionMark;
    private Transform playerCamera;

    private float rotateSpeed = 20;
    private string message;

    private Vector3 targetSize;
    private float spawnTime = 1;
    #endregion

    #region Initialization
    static void Init() {
        if (!initialized) {
            hintPrefab = Resources.Load<GameObject>("Prefabs/HintBox");
            hintTextPrefab = Resources.Load<GameObject>("Prefabs/FloatingHint");

            positions = GameObject.FindGameObjectsWithTag("Hint").Select(o => o.transform.position).ToArray();

            initialized = true;
        }
    }

    protected override void Awake_DragAcceptable() {
        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    protected override void Start_Interactable() {
        Type.On(InteractableType.Interactable, InteractableType.Draggable);

        playerCamera = Player.Camera.transform;

        questionMark = transform.Find("Question Mark");
        StartCoroutine(InitHintBox());
    }

    private IEnumerator InitHintBox() {

        float time = spawnTime;

        while (time > 0) {
            time -= Time.deltaTime;
            float factor = 1 - time / spawnTime;
            transform.localScale = targetSize * factor;
            yield return null;
        }
        transform.localScale = targetSize;
    }
    #endregion

    protected override void Update() {
        base.Update();
        RotateBox();
    }
    
    private void RotateBox() {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        questionMark.LookAt(playerCamera);
    }

    protected override void Activate() {

        if (Activated) {
            return;
        }

        Activated = true;

        GameObject newHintText = Instantiate(hintTextPrefab);

        currentHintText = newHintText.GetComponent<HintText>();

        newHintText.transform.position = startPos;
        newHintText.transform.LookAt(Player.Camera.transform);

        TextMeshPro text = newHintText.transform.Find("Text").GetComponent<TextMeshPro>();

        text.text = message;
        grabbed = false;
        SafeDestroy();
    }
    #region Creating
    public static void CreateHint(string message) {

        if (currentHintText != null) {
            currentHintText.DestroyHint();
        }

        if (currentHint != null) {
            currentHint.message = message;
            return;
        }

        Init();

        Vector3 hintPos = GetHintPosition();

        GameObject newHint = Instantiate(hintPrefab);
        newHint.transform.position = hintPos;

        HintBox hint = newHint.GetComponent<HintBox>();
        currentHint = hint;

        hint.message = message;
    }

    private static Vector3 GetHintPosition() {

        Vector3 currentPos = Player.Camera.transform.position;

        List<Vector3> possible = new List<Vector3>();

        foreach (Vector3 pos in positions) {
            if (Vector3.Distance(currentPos, pos) < maxDistance) {
                possible.Add(pos);
            }
        }
        if (possible.Count == 0) {
            Logger.Warning("No suitable position was found, returning the default one");
            return positions[0];
        }

        return GetClosestPosition(possible);
    }
    private static Vector3 GetClosestPosition(List<Vector3> positions) {

        Vector3 forward = Player.Camera.transform.forward;
        Vector3 camPos = Player.Camera.transform.position;

        float smallestAngle = float.MaxValue;
        Vector3 pos = Vector3.zero;

        foreach (Vector3 p in positions) {
            float angle = Vector3.Angle(p - camPos, forward);

            Logger.PrintVariables("angle", angle, "pos", p, "smallest", smallestAngle, "smalles pos", pos);

            if (angle < smallestAngle) {
                smallestAngle = angle;
                pos = p;
            }
        }

        return pos;
    }

    // Currently unused but might be helpful
    private static bool InViewLimit(Vector3 pos) {

        Vector3 view = Player.Camera.WorldToViewportPoint(pos);

        if (view.z <= 0) {
            return false;
        }
        if (view.x > 1 || view.x < viewLimitX) {
            return false;
        }
        if (view.y > 1 || view.y < viewLimitY) {
            return false;
        }

        return true;
    }

    // Currently unused but might be helpful
    private static bool IsVisible(Vector3 pos) {

        Vector3 camPos = Player.Camera.transform.position;

        float distance = Vector3.Distance(pos, camPos);
        Vector3 direction = camPos - pos;

        Ray ray = new Ray(pos, direction);

        return !Physics.Raycast(ray, distance);
    }
    #endregion
}
