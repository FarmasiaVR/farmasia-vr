using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class CoverOpeningHandler : MonoBehaviour {
    public bool actionRequired;
    public bool actionComplete;
    private Cover itemCover;
    public UnityEvent onActionComplete;
    void Start() {
        itemCover = this.GetComponent<Cover>();
    }

    public void completeAction() {
        actionComplete = true;
    }
    public void openPackage() {
        if (actionRequired && actionComplete) {
            Debug.Log("Yes way");
            onActionComplete?.Invoke();
        } else Debug.Log("No way");
    }
    public void openPackageWrong() {
        if (actionRequired && actionComplete)
        Debug.Log("Wrong end of package maybe by mistake???");
        openPackage();
    }
}