using UnityEngine;

public class Door : MonoBehaviour {

    UIWriter writer;
    public GameObject warpPoint;
    private GameObject player = null;

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.name == "Player") {
            writer.toggleChild("Prompt Field");
            player = collision.gameObject;
        }
    }

    void OnTriggerExit(Collider collision) {
        if (collision.gameObject.name == "Player") {
            writer.toggleChild("Prompt Field");
            player = null;
        }
    }

    void Start() {
        writer = GameObject.Find("Canvas").GetComponent<UIWriter>();
    }

    void Update() {
        if (player) {
            if (Input.GetKeyUp(KeyCode.F)) {
                player.transform.position = warpPoint.transform.position;
            }
        }
    }
}
