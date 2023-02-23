using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HintBox : DragAcceptable {

    private static GameObject hintPrefab;
    private static GameObject hintTextPrefab;
    private static HintBox boxInstance;
    private static HintText hintInstance;
    private static Vector3[] positions;
    private static bool initialized = false;
    private static float maxDistance = 5f;

    private Transform playerCamera;
    private Transform questionMark;
    private Vector3 targetSize;

    private bool hintBoxOpened;
    private float rotateSpeed = 20;
    private float spawnTime = 1;
    private string message;

    public static void Init(bool force = false) {
        if (!initialized || force) {
            hintPrefab = Resources.Load<GameObject>("Prefabs/HintBox");
            hintTextPrefab = Resources.Load<GameObject>("Prefabs/FloatingHint");
            positions = GameObject.FindGameObjectsWithTag("Hint").Select(o => o.transform.position).ToArray();
            initialized = true;
        }
    }

    protected override void Awake() {
        base.Awake();
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

    protected override void Update() {
        base.Update();
        RotateBox();
    }

    private void RotateBox() {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        questionMark.LookAt(2 * questionMark.position - playerCamera.position);
    }

    protected override void Activate() {
        if (!hintBoxOpened) {
            Task.CreateGeneralMistake("Vinkkilaatikko avattiin", 2, false);
            hintBoxOpened = true;
        }

        if (ActivateCount > 0) {
            return;
        }

        CreateHintText(G.Instance.Progress.CurrentPackage.CurrentTask.Hint, startPos);
        grabbed = false;
        SafeDestroy();
        boxInstance = null;
    }

    private static void CreateHintText(string message, Vector3 position) {
        GameObject newHintText = Instantiate(hintTextPrefab);
        hintInstance = newHintText.GetComponent<HintText>();
        newHintText.transform.position = position;
        newHintText.transform.LookAt(Player.Camera.transform);
        hintInstance.Text = message;
        TextMeshPro text = newHintText.transform.Find("Text").GetComponent<TextMeshPro>();
        text.text = message;
    }

    public static void CreateHint(string message, bool open = false) {
        if (hintInstance != null) {
            if (hintInstance.Text.Equals(message)) {
                return;
            } else {
                hintInstance.DestroyHint();
            }
        }

        if (boxInstance != null) {
            boxInstance.message = message;
            if (Vector3.Distance(boxInstance.transform.position, Player.Camera.transform.position) > maxDistance) {
                boxInstance.SafeDestroy();
                boxInstance = null;
            } else {
                return;
            }
        }

        Init();
        Vector3 hintPos = GetHintPosition();

        if (open) {
            CreateHintText(message, hintPos);
            return;
        }

        GameObject newHint = Instantiate(hintPrefab);
        newHint.transform.position = hintPos;
        HintBox hint = newHint.GetComponent<HintBox>();
        boxInstance = hint;
        hint.message = message;
    }

    public static void DestroyCurrentHint() {
        if (hintInstance != null) {
            hintInstance.DestroyHint();
        }

        if (boxInstance != null) {
            boxInstance.SafeDestroy();
            boxInstance = null;
        }
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

            if (angle < smallestAngle) {
                smallestAngle = angle;
                pos = p;
            }
        }

        return pos;
    }

    public void XRInteract()
    {
        Activate();
    }
}
