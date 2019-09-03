using UnityEngine;
using UnityEditor;

public class AutoSnap : EditorWindow {
    private Vector3 prevPosition;
    private Vector3 prevRotation;
    private Vector3 prevScale;

    private bool snapPos = false;
    private float posSnapValue = 0.25f;

    private bool snapRot = false;
    private float rotSnapValue = 15;

    private bool snapScale = false;
    private float scaleSnapValue = 0.25f;

    private enum SnapType {
        Pos, Rot, Scale
    }

    [MenuItem("Edit/Auto Snap %_l")]

    static void Init() {
        var window = (AutoSnap)EditorWindow.GetWindow(typeof(AutoSnap));
        window.maxSize = new Vector2(200, 100);
    }

    public void OnGUI() {
        snapPos = EditorGUILayout.Toggle("Auto Snap Position", snapPos);
        posSnapValue = EditorGUILayout.FloatField("Pos Snap Value", posSnapValue);

        snapRot = EditorGUILayout.Toggle("Auto Snap Rotation", snapRot);
        rotSnapValue = EditorGUILayout.FloatField("Rot Snap Value", rotSnapValue);

        snapScale = EditorGUILayout.Toggle("Auto Snap Scale", snapScale);
        scaleSnapValue = EditorGUILayout.FloatField("Scale Snap Value", scaleSnapValue);
    }

    public void Update() {

        if (!EditorApplication.isPlaying
            && Selection.transforms.Length > 0) {

            if (snapPos && Selection.transforms[0].position != prevPosition) {
                Snap(SnapType.Pos);
                prevPosition = Selection.transforms[0].position;
            }

            if (snapRot && Selection.transforms[0].eulerAngles != prevRotation) {
                Snap(SnapType.Rot);
                prevRotation = Selection.transforms[0].eulerAngles;
            }

            if (snapScale && Selection.transforms[0].localScale != prevScale) {
                Snap(SnapType.Scale);
                prevScale = Selection.transforms[0].localScale;
            }
        }
    }

    private void Snap(SnapType type) {
        foreach (var transform in Selection.transforms) {

            switch (type) {
                case SnapType.Pos:
                    transform.transform.position = SnapVector(transform.transform.position, posSnapValue);
                    break;
                case SnapType.Rot:
                    transform.transform.eulerAngles = SnapVector(transform.transform.eulerAngles, rotSnapValue);
                    break;
                case SnapType.Scale:
                    transform.transform.localScale = SnapVector(transform.transform.localScale, scaleSnapValue);
                    break;

            }
        }
    }

    private Vector3 SnapVector(Vector3 vector, float snap) {
        vector.x = Round(vector.x, snap);
        vector.y = Round(vector.y, snap);
        vector.z = Round(vector.z, snap);

        return vector;
    }

    private float Round(float input, float snap) {
        return posSnapValue * Mathf.Round((input / posSnapValue));
    }
}