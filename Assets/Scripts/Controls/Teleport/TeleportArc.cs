using System.Collections.Generic;
using UnityEngine;

public class TeleportArc {

    #region fields
    private const float distance = 0.1f;
    private const float acceleration = 0.2f;
    private const float angle = 1f;
    private const float maxLineDistance = 20;
    #endregion

    public void ShootArc(Vector3 startPos, Vector3 startDir, out Vector3[] outArc, out GameObject hitObject, out Vector3 hitPos) {
        List<Vector3> arc = new List<Vector3>();
        float currentDistance = 0;

        arc.Add(startPos);

        hitObject = null;
        hitPos = Vector3.zero;

        Vector3 prevPos = startPos;

        for (int i = 1; currentDistance < maxLineDistance; i++) {
            Vector3 endPos = PositionByIndex(startPos, startDir, i);

            RaycastHit rayHit;
            bool didHit = CastRay(prevPos, endPos, out rayHit);

            prevPos = endPos;
            arc.Add(endPos);

            if (didHit) {
                hitObject = rayHit.collider.gameObject;
                hitPos = rayHit.point;
                break;
            }

            currentDistance += distance;
        }

        outArc = arc.ToArray();
        arc = null;
    }
    private void NextRay(ref Vector3 pos, ref Vector3 dir) {
        pos += dir.normalized * distance;
        //dir = Quaternion.AngleAxis(angle, Vector3.down).eulerAngles;
    }

    private Vector3 PositionByIndex(Vector3 startPos, Vector3 startDir, int index) {
        float x = (index * distance);
        float y = Mathf.Pow(acceleration * x, 2);

        return startPos + x * startDir.normalized + y * Vector3.down;
    }

    private bool CastRay(Vector3 pos, Vector3 endPos, out RaycastHit hit) {
        Vector3 dir = endPos - pos;

        if (Physics.Raycast(pos, dir, out hit, distance)) {
            return true;
        }

        return false;
    }
}