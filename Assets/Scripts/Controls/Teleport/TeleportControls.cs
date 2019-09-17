using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControls : MonoBehaviour {

    #region fields
    private TeleportArc arc;
    private LineRenderer line;

    private bool teleporting;

    private bool invalidTeleport;
    private Vector3 hitPos;

    private Transform player;
    #endregion


    private void Start() {
        line = GetComponent<LineRenderer>();

        arc = new TeleportArc();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void StartTeleport() {
        teleporting = true;
    }
    public void EndTeleport() {
        teleporting = false;

        line.positionCount = 0;

        TeleportToPosition();
    }

    private void Update() {
        if (teleporting) {
            DrawLine();
        }
    }

    private void DrawLine() {

        GameObject hit;
        Vector3[] positions;

        arc.ShootArc(transform.position, transform.forward, out positions, out hit, out hitPos);

        invalidTeleport = hit == null || hit.gameObject.tag != "Floor";

        Logger.PrintVariables("count", positions.Length);

        line.positionCount = positions.Length;
        line.SetPositions(positions);
    }

    private void TeleportToPosition() {

        if (invalidTeleport) {
            Logger.Print("Invalid teleport");
            return;
        }

        player.transform.position = hitPos;
    }
}
