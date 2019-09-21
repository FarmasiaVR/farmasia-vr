using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UISystem : MonoBehaviour {

    #region fields
    public static UISystem Instance { get; private set; }
    private List<GameObject> popUps = new List<GameObject>();
    [SerializeField]
    private GameObject popupPrefab;
    private GameObject cameraRig;
    #endregion

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    public void DeleteChild(GameObject popupObject) {
        popUps.Remove(popupObject);
    }

    public void CreatePopup(int point, string message, MessageType type) {
        GameObject popupMessage = Instantiate(popupPrefab, new Vector3(0, 0, 10), Quaternion.identity);
        popupMessage.GetComponent<PointPopup>().setPopup(point, message, type);
        popUps.Add(popupMessage);
    }
}
