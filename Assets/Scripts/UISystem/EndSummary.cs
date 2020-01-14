using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSummary : MonoBehaviour {

    #region fields
    private const string TAG = "EndSummary";
    #endregion

    private void Start() {
        SetChildStatuses(false);
    }

    private void SetChildStatuses(bool status) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(status);
        }
    }

    private void EnableSummary(string summary) {
        SetChildStatuses(true);

        TextMeshPro text = transform.Find("Text").GetComponent<TextMeshPro>();

        if (text == null) {
            Logger.Error("Text mesh pro was null in end summary");
            return;
        }

        text.text = summary;
    }

    public static void EnableEndSummary(string summary) {
        GameObject.FindGameObjectWithTag(TAG).GetComponent<EndSummary>().EnableSummary(summary);
    }
}