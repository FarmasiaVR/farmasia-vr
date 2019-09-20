using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour {

    public GameObject popup;

    public static UISystem Instance { get; private set; }
    List<GameObject> popUps = new List<GameObject>();
    GameObject blankPoint;
    GameObject cameraRig;

    void Start() {
        blankPoint = transform.Find("Blank").gameObject;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }
    public void deleteChild(GameObject popupObject) {
        popUps.Remove(popupObject);
    }
    public void CreatePopup(int point, string message, MessageType type) {
        GameObject popupMessage = Instantiate(popup, new Vector3(0, 0, 10), Quaternion.identity);
        popupMessage.GetComponent<PointPopup>().setPopup(point, message, type);
        popUps.Add(popupMessage);
    }
}
