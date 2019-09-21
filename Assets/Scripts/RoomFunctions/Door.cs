using UnityEngine;

public class Door : MonoBehaviour {

    #region fields
    public GameObject warpPoint;
    private UIWriter writer;
    private GameObject player = null;
    #endregion

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.name == "Player") {
            writer.ToggleChild("Prompt Field");
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.gameObject.name == "Player") {
            writer.ToggleChild("Prompt Field");
            player = null;
        }
    }

    private void Start() {
        writer = GameObject.Find("Canvas").GetComponent<UIWriter>();
    }

    private void Update() {
        if (player) {
            if (Input.GetKeyUp(KeyCode.F)) {
                player.transform.position = warpPoint.transform.position;
            }
        }
    }
}
