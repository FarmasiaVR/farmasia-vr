using System.Collections.Generic;
using UnityEngine;

public class TeleportArc {

    #region Constants
    private const float DISTANCE = 0.1f;
    private const float ACCELERATION = 0.2f;
    private const float ANGLE = 1f;
    private const float MAX_LINE_DISTANCE = 100;
    #endregion

    #region Fields
    private float hitHeight;
    private int layer;
    #endregion

    public TeleportArc() {
        layer = LayerMask.NameToLayer("Default");
    }

    public void ShootArc(Vector3 startPos, Vector3 startDir, out Vector3[] outArc, out GameObject hitObject, out Vector3 hitPos) {
        List<Vector3> arc = new List<Vector3>();
        float currentDistance = 0;

        arc.Add(startPos);

        hitObject = null;
        hitPos = Vector3.zero;

        Vector3 prevPos = startPos;

        for (int i = 1; currentDistance < MAX_LINE_DISTANCE; i++) {
            Vector3 endPos = PositionByIndex(startPos, startDir, i);

            RaycastHit rayHit;
            bool didHit = CastRay(prevPos, endPos, out rayHit);

            prevPos = endPos;
            arc.Add(endPos);

            //if (didHit) {
            //    hitObject = rayHit.collider.gameObject;
            //    hitPos = rayHit.point;
            //    break;
            //}

            if (endPos.y < hitHeight) {
                endPos.y = hitHeight;
                hitPos = endPos;
                break;
            }

            currentDistance += DISTANCE;
        }

        outArc = arc.ToArray();
        arc = null;
    }
    private void NextRay(ref Vector3 pos, ref Vector3 dir) {
        pos += dir.normalized * DISTANCE;
        //dir = Quaternion.AngleAxis(angle, Vector3.down).eulerAngles;
    }

    private Vector3 PositionByIndex(Vector3 startPos, Vector3 startDir, int index) {
        float x = (index * DISTANCE);
        float y = Mathf.Pow(ACCELERATION * x, 2);

        return startPos + x * startDir.normalized + y * Vector3.down;
    }

    private bool CastRay(Vector3 pos, Vector3 endPos, out RaycastHit hit) {
        Vector3 dir = endPos - pos;

        if (Physics.Raycast(pos, dir, out hit, DISTANCE, layer, QueryTriggerInteraction.Ignore)) {
            return true;
        }

        return false;
    }
}