using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintBox : Interactable {

    #region Fields
    private Transform questionMark;
    private Transform playerCamera;

    private float rotateSpeed = 20;
    #endregion

    protected override void Start() {
        base.Start();

        questionMark = transform.Find("Question Mark");

        // Not actual camera, just the player object
        playerCamera = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update() {
        RotateBox();
    }

    private void RotateBox() {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        questionMark.LookAt(playerCamera);
    }
}
