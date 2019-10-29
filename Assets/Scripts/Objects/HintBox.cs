using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintBox : Interactable {

    #region Fields
    private static float defaultDistance = 0.5f;
    private static float viewLimitX = 0.8f;
    private static float viewLimitY = 0.6f;

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
        if (hintPrefab == null || hintTextPrefab == null) {
            hintPrefab = Resources.Load<GameObject>("Prefabs/HintBox");
            hintTextPrefab = Resources.Load<GameObject>("Prefabs/FloatingHint");
        }
    }

    private void Awake() {
        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    protected override void Start() {
        base.Start();

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

    private void Update() {
        RotateBox();
    }
    
    private void RotateBox() {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        questionMark.LookAt(playerCamera);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);

        Logger.PrintVariables("message", message);
        if (Interactors.Hand != null) {
            Interactors.Hand.ReleaseObject();
        }

        ActivateHint();

        DestroyInteractable();
    }
    public void ActivateHint() {

        GameObject newHintText = Instantiate(hintTextPrefab);

        newHintText.transform.position = transform.position;
        newHintText.transform.LookAt(Player.Camera.transform);

        TextMeshPro text = newHintText.transform.Find("Text").GetComponent<TextMeshPro>();

        text.text = message;
    }
    #region Creating
    public static void CreateHint(string message) {

        Init();

        Vector3 hintPos = GetPositionInView();

        GameObject newHint = Instantiate(hintPrefab);
        newHint.transform.position = hintPos;

        HintBox hint = newHint.GetComponent<HintBox>();
        hint.message = message;
    }

    private static Vector3 GetPositionInView() {

        Vector3 r = Vector3.zero;

        int failsafe = 1000000;

        while (true) {
            failsafe--;
            if (failsafe < 0) {
                Logger.Error("No view found");
                return r;
            }
            r = Player.Camera.transform.position + RandomVector;
            if (InViewLimit(r) && IsVisible(r)) {
                return r;
            }
        }
    }


   // Alternatively, define few possible hint positions and pick one from them
    //private static Vector3 RandomVector(Transform from) {

    //    float Random() {
    //        return UnityEngine.Random.value - 0.5f;
    //    }

    //    return from.right * Random() + from.up *
    //}
    private static Vector3 RandomVector {
        get {
            return new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value * 0.5f, 0.5f).normalized * defaultDistance;
        }
    }
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
    private static bool IsVisible(Vector3 pos) {

        Vector3 camPos = Player.Camera.transform.position;

        float distance = Vector3.Distance(pos, camPos);
        Vector3 direction = camPos - pos;

        Ray ray = new Ray(pos, direction);

        return !Physics.Raycast(ray, distance);
    }
    #endregion
}
