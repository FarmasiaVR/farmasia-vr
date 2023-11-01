using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour {
    [SerializeField] private GameObject xrPlayer;
    [SerializeField] private GameObject xrOrigin;
    private Dictionary<string, GameObject> namedTpPoints = new Dictionary<string, GameObject>();
    
    void Start() {
        GameObject[] tpPoints = GameObject.FindGameObjectsWithTag("TeleportPoint");

        foreach (GameObject tpPoint in tpPoints) {
            string name = tpPoint.GetComponent<TeleportPointData>().teleportPointName;
            namedTpPoints[name] = tpPoint;
            Debug.Log($"Added {name}"); 
        }
    }

    // This is a workaround that shouldn't be needed for teleportation to work
    private void ResetXrOrigin() {
        xrOrigin.transform.localPosition = new Vector3(0, 0, 0);
        xrOrigin.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void TeleportPlayer(string name) {
        if (namedTpPoints.ContainsKey(name)) {
            xrPlayer.transform.position = namedTpPoints[name].transform.position;
            xrPlayer.transform.rotation = namedTpPoints[name].transform.rotation;
            ResetXrOrigin();
        } else {
            Debug.LogWarning($"Teleport point {name} not found.");
        }
    }
}
