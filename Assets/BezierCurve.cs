using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{
    private int FixedPoints = 3;

    [SerializeField, Range(1, 3)] private int detail = 1;
    [SerializeField, Range(0, 0.5f)] private float smoothness = 0.25f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<Transform> ControlPoints;
    
    private Vector3[] positions;

    private int vertices;

    void OnValidate() {
        UpdatePositions();
        //GenerateBezier();
    }

    void Update() {
        UpdatePositions();
    }

    private void UpdatePositions() {
        vertices = FixedPoints + (FixedPoints - 1) * detail;

        positions = new Vector3[ControlPoints.Count];
        for (int i = 0; i < positions.Length; i++) {
            positions[i] = ControlPoints[i].position;
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    private void GenerateBezier() {
        List<Vector3> points = new List<Vector3>();
        for (int i = 2; i < positions.Length; i++) {
            points.AddRange(
                CurveBetween(positions[i-2], positions[i-1], positions[i], vertices - 1)
            );
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private void SmoothCorners() {
        int cornersToSmooth = FixedPoints - 2;
        
        Vector3[] points = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(points);
        
        Vector3[] newPoints = new Vector3[lineRenderer.positionCount + (cornersToSmooth * 2)];
        
        for (int i = 0; i < points.Length; i++) {
            if (i % (detail + 1) != 0) {
                newPoints[i] = points[i];
                continue;
            }

            
        }
    }

    private Vector3[] CurveBetween(Vector3 pos1, Vector3 pos2, Vector3 pos3, int steps) {
        List<Vector3> points = new List<Vector3>();
        for (float delta = 0; delta <= 1; delta += 1.0f / steps) {
            Vector3 result = Vector3.Lerp(
                Vector3.Lerp(pos1, pos2, delta),
                Vector3.Lerp(pos2, pos3, delta),
                delta
            );
            points.Add(result);
        }
        return points.ToArray();
    }
}
